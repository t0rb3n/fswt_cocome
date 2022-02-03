using System.Net.NetworkInformation;
using CashDesk.BankServer;
using CashDesk.BarcodeScannerService;
using CashDesk.CardReaderService;
using CashDesk.CashboxService;
using CashDesk.DisplayController;
using CashDesk.Exceptions;
using CashDesk.PrintingService;
using CashDesk.TransferObjects;
using Tecan.Sila2;
using Tecan.Sila2.Client;
using Tecan.Sila2.Client.ExecutionManagement;
using Tecan.Sila2.Discovery;

namespace CashDesk;

public class CashDesk
{
    private readonly EnterpriseRpc.EnterpriseRpcClient _enterpriseclient;

    private static readonly string InvalidCardInfo = "XXXX XXXX XXXX XXXX";
    private CashboxServiceClient _cashboxClient;
    private DisplayControllerClient _displayClient;
    private PrintingServiceClient _printerClient;
    private BarcodeScannerServiceClient _barcodeClient;
    private CardReaderServiceClient _cardReaderClient;
    private BankServerClient _bankClient;
    private double _currentRunningTotal;
    private CashDeskState _currentState = CashDeskState.ExpectingSale;
    private bool _expressModeEnabled = false;
    private List<ProductWithStockItemTO> _saleProducts;
    private string _cardInfo;

    // New sale can be started anytime and thus aborted expect when we already paid by cash 
    private static readonly HashSet<CashDeskState> StartSaleStates = new HashSet<CashDeskState>
    {
        CashDeskState.ExpectingSale,
        CashDeskState.ExpectingItems,
        CashDeskState.ExpectingPayment,
        CashDeskState.PayingByCash,
        CashDeskState.ExpectingCardInfo,
        CashDeskState.PayingByCreditCard
    };

    public CashDesk()
    {
        var connector = new ServerConnector(new DiscoveryExecutionManager());
        var discovery = new ServerDiscovery(connector);
        var servers = discovery.GetServers(TimeSpan.FromSeconds(10),
            n => n.NetworkInterfaceType == NetworkInterfaceType.Loopback);
        var terminalServer = servers.First(s => s.Info.Type == "Terminal");
        var bankServer = servers.FirstOrDefault(s => s.Info.Type == "BankServer");
        var executionManagerFactory = new ExecutionManagerFactory(Enumerable.Empty<IClientRequestInterceptor>());
        var terminalServerExecutionManager = executionManagerFactory.CreateExecutionManager(terminalServer);

        _cashboxClient = new CashboxServiceClient(terminalServer.Channel, terminalServerExecutionManager);
        _displayClient = new DisplayControllerClient(terminalServer.Channel, terminalServerExecutionManager);
        _printerClient = new PrintingServiceClient(terminalServer.Channel, terminalServerExecutionManager);
        _barcodeClient = new BarcodeScannerServiceClient(terminalServer.Channel, terminalServerExecutionManager);
        _cardReaderClient = new CardReaderServiceClient(terminalServer.Channel, terminalServerExecutionManager);
        _bankClient =
            new BankServerClient(bankServer?.Channel, executionManagerFactory.CreateExecutionManager(bankServer));
        _currentRunningTotal = 0;
    }

    async void WaitForSaleStarted(
        IIntermediateObservableCommand<CashboxButton> cashboxButtons
    )
    {
        while (await cashboxButtons.IntermediateValues.WaitToReadAsync())
        {
            if (!cashboxButtons.IntermediateValues.TryRead(out
                    var button)) continue;
            if (button == CashboxButton.StartNewSale)
            {
                StartSale();
            }
        }
    }

    // upon starting we shall listen to the keys pressed. 
    // if a key is pressed and we are not in the state "ExpectingSale", we throw an IllegalStateException.
    private void StartSale()
    {
        this.CheckState(StartSaleStates);
        //TODO send SALESTARTEDEVENT ? 
        this._currentState = CashDeskState.ExpectingItems;
        this.ResetSale();
    }

    private void CheckState(IReadOnlySet<CashDeskState> legalStates)
    {
        if (!legalStates.Contains(this._currentState))
        {
            throw new IllegalCashDeskStateException(this._currentState, legalStates);
        }
    }

    private void ResetSale()
    {
        this._currentRunningTotal = 0.0;
        this._saleProducts = new List<ProductWithStockItemTO>();
        this._cardInfo = InvalidCardInfo;
    }

    void listenToSaleStartedEvent()
    {
        throw new NotImplementedException();
    }

    void listenToSaleFinishedEvent()
    {
        throw new NotImplementedException();
    }

    void listenToProductBarCodeScannedEvent()
    {
        throw new NotImplementedException();
    }

    ProductWithStockItem getProductWithStockItem(long barcode)
    {
        return _enterpriseclient.GetProductWithStockItem(new Barcode {Code = barcode});
    }
}