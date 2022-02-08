using System.Net.NetworkInformation;
using CashDesk.BankServer;
using CashDesk.BarcodeScannerService;
using CashDesk.CardReaderService;
using CashDesk.CashboxService;
using CashDesk.DisplayController;
using CashDesk.PrintingService;
using Tecan.Sila2.Client;
using Tecan.Sila2.Client.ExecutionManagement;
using Tecan.Sila2.Discovery;

namespace CashDesk;

public sealed class CashDeskEventPublisher
{
    // Cash Box Button Events
    public event EventHandler? StartSale;
    public event EventHandler? FinishSale;
    public event EventHandler? PayWithCard;
    public event EventHandler? PayWithCash;
    public event EventHandler? DisableExpressMode;

    // Item Events
    public event EventHandler<string> AddItemToSale;
    


    private CashboxServiceClient _cashboxClient;
    private DisplayControllerClient _displayClient;
    private PrintingServiceClient _printerClient;
    private BarcodeScannerServiceClient _barcodeClient;
    private CardReaderServiceClient _cardReaderClient;
    private BankServerClient _bankClient;

    public CashDeskEventPublisher()
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
    }

    public async void StartListeningToTerminal()
    {
        var cashboxButtons = _cashboxClient.ListenToCashdeskButtons();
        while (await cashboxButtons.IntermediateValues.WaitToReadAsync())
        {
            if (!cashboxButtons.IntermediateValues.TryRead(out
                    var button)) continue;
            switch (button)
            {
                case CashboxButton.StartNewSale:
                    OnStartSaleEvent(EventArgs.Empty);
                    StartListeningToBarcodes();
                    break;
                case CashboxButton.FinishSale:
                    Console.WriteLine("FinishSale");
                    throw new NotImplementedException();

                case CashboxButton.DisableExpressMode:
                    Console.WriteLine("DisableExpressMode");
                    throw new NotImplementedException();

                case CashboxButton.PayWithCard:
                    Console.WriteLine("PayWithCard");
                    throw new NotImplementedException();

                case CashboxButton.PayWithCash:
                    Console.WriteLine("PayWithCash");
                    throw new NotImplementedException();

                default:
                    Console.WriteLine("Default");
                    throw new NotImplementedException();
            }
        }
    }

    private async void StartListeningToBarcodes()
    {
        var barcodes = _barcodeClient.ListenToBarcodes();

        while (await barcodes.IntermediateValues.WaitToReadAsync())
        {
            if (!barcodes.IntermediateValues.TryRead(out var barcode)) continue;

            OnAddItemToSaleEvent(barcode);
        }
    }

    private void OnStartSaleEvent(EventArgs e)
    {
        StartSale?.Invoke(this, e);
    }

    private void OnAddItemToSaleEvent(string barcode)
    {
        AddItemToSale.Invoke(this, barcode);
    }

    private void OnFinishSale(EventArgs e)
    {
        FinishSale?.Invoke(this, e);
    }

    private void OnDisableExpressMode(EventArgs e)
    {
        DisableExpressMode?.Invoke(this, e);
    }

    private void OnPayWithCard(EventArgs e)
    {
        PayWithCard?.Invoke(this, e);
    }

    private void OnPayWithCash(EventArgs e)
    {
        PayWithCash?.Invoke(this, e);
    }
}