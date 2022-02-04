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
    private readonly ILogger<CashDesk> _logger;


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
    private List<ProductWithStockItem> _saleProducts; // TODO use TO Objects
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

    private static readonly HashSet<CashDeskState> AddItemToSaleStates = new HashSet<CashDeskState>
    {
        CashDeskState.ExpectingItems
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
        this.StateIsLegal(StartSaleStates);
        //TODO send SALESTARTEDEVENT ? 
        this._currentState = CashDeskState.ExpectingItems;
        this.ResetSale();
    }

    private void StateIsLegal(IReadOnlySet<CashDeskState> legalStates)
    {
        if (!legalStates.Contains(this._currentState))
        {
            throw new IllegalCashDeskStateException(this._currentState, legalStates);
        }
    }

    private void ResetSale()
    {
        _currentRunningTotal = 0.0;
        _saleProducts = new List<ProductWithStockItem>();
        _cardInfo = InvalidCardInfo;
    }

    private void AddItemToSale(long barcode)
    {
        StateIsLegal(AddItemToSaleStates);

        if (CanAcceptItem())
        {
            try
            {
                ProductWithStockItem productWithStockItem = GetProductWithStockItem(barcode);
                AddItemToSale(productWithStockItem);
            }
            catch (NoSuchProductException nspe)
            {
                _logger.LogError("No product/stock item for barcode ${Barcode}", barcode);
                //TODO  send this.sendInvalidProductBarcodeEvent(barcode);
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to communicate with store server");
            }
        }
        else
        {
            // TODO check/discuss what to do when we try to add more than policy allows
            // TODO show message in display
            _logger.LogError("Cannot process more than ${Number} items in express mode!",
                ExpressModePolicy._expressItemsLimit);
        }
    }

    // If we are in express mode, you are only allowed to have a maximum of 8 Items
    private bool CanAcceptItem()
    {
        bool expressModeDisabled = !this._expressModeEnabled;
        bool itemCountUnderLimit = this._saleProducts.Count < ExpressModePolicy._expressItemsLimit;
        return expressModeDisabled || itemCountUnderLimit;
    }

    private void AddItemToSale(ProductWithStockItem product)
    {
        _saleProducts.Add(product);
        double currentTotal = CalculateCurrentTotal(product.Price);
        DisplayCurrentTotal(currentTotal);
    }

    private double CalculateCurrentTotal(double price) => _currentRunningTotal += price;

    private void DisplayCurrentTotal(double total)
    {
        // TODO send the new total to the Terminal and let it show.
        throw new NotImplementedException();
    }

    private void listenToSaleStartedEvent()
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

    private ProductWithStockItem GetProductWithStockItem(long barcode)
    {
        return _enterpriseclient.GetProductWithStockItem(new Barcode {Code = barcode});
    }
}