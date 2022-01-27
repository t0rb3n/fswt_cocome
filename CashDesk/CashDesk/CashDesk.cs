using System.Net.NetworkInformation;
using CashDesk.BankServer;
using CashDesk.BarcodeScannerService;
using CashDesk.CardReaderService;
using CashDesk.CashboxService;
using CashDesk.DisplayController;
using CashDesk.PrintingService;
using Grpc.Core;
using Tecan.Sila2.Client;
using Tecan.Sila2.Client.ExecutionManagement;
using Tecan.Sila2.Discovery;

namespace CashDesk;

public class CashDesk
{
    private CashboxServiceClient _cashboxClient;
    private DisplayControllerClient _displayClient;
    private PrintingServiceClient _printerClient;
    private BarcodeScannerServiceClient _barcodeClient;
    private CardReaderServiceClient _cardReaderClient;
    private BankServerClient _bankClient;
    private double _currentRunningTotal = 0;
    private CashDeskState currentState = CashDeskState.ExpectingSale;

    public CashDesk()
    {
        var connector = new ServerConnector(new DiscoveryExecutionManager());
        var discovery = new ServerDiscovery(connector);
        var servers = discovery.GetServers(TimeSpan.FromSeconds(10), n => n.NetworkInterfaceType == NetworkInterfaceType.Loopback);
        var terminalServer = servers.First(s => s.Info.Type == "Terminal");
        var bankServer = servers.FirstOrDefault(s => s.Info.Type == "BankServer");
        var executionManagerFactory = new ExecutionManagerFactory(Enumerable.Empty<IClientRequestInterceptor>());
        var terminalServerExecutionManager = executionManagerFactory.CreateExecutionManager(terminalServer);
        
        _cashboxClient = new CashboxServiceClient(terminalServer.Channel, terminalServerExecutionManager);
        _displayClient = new DisplayControllerClient(terminalServer.Channel, terminalServerExecutionManager);
        _printerClient = new PrintingServiceClient(terminalServer.Channel, terminalServerExecutionManager);
        _barcodeClient = new BarcodeScannerServiceClient(terminalServer.Channel, terminalServerExecutionManager);
        _cardReaderClient = new CardReaderServiceClient(terminalServer.Channel, terminalServerExecutionManager);
        _bankClient = new BankServerClient(bankServer?.Channel, executionManagerFactory.CreateExecutionManager(bankServer));
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

    void getProductWithStockItem(long barcode)
    {
        throw new NotImplementedException();
    }
    

}