using System.Net.NetworkInformation;
using CashDesk;
using CashDesk.Application.CashDesk;
using CashDesk.Application.CashDesk.EventHandler;
using CashDesk.Application.Interfaces;
using CashDesk.BankServer;
using CashDesk.BarcodeScannerService;
using CashDesk.CardReaderService;
using CashDesk.CashboxService;
using CashDesk.DisplayController;
using CashDesk.Infrastructure.Services;
using CashDesk.Infrastructure.Sila.BankServer;
using CashDesk.Infrastructure.Sila.DisplayController;
using CashDesk.Infrastructure.Sila.PrintingService;
using CashDesk.PrintingService;
using GrpcModule.Services.Store;
using Tecan.Sila2;
using Tecan.Sila2.Client;
using Tecan.Sila2.Client.ExecutionManagement;
using Tecan.Sila2.Discovery;


var connector = new ServerConnector(new DiscoveryExecutionManager());
var discovery = new ServerDiscovery(connector);
var servers = discovery.GetServers(TimeSpan.FromSeconds(10),
    n => n.NetworkInterfaceType == NetworkInterfaceType.Loopback);
ServerData terminalServer;
ServerData bankServer;
try
{
    terminalServer = servers.First(s => s.Info.Type == "Terminal");
    bankServer = servers.FirstOrDefault(s => s.Info.Type == "BankServer");
}
catch (InvalidOperationException)
{
    Console.WriteLine("Banking or Terminal server appears to be offline.");
    throw;
}

var executionManagerFactory = new ExecutionManagerFactory(Enumerable.Empty<IClientRequestInterceptor>());
var terminalServerExecutionManager = executionManagerFactory.CreateExecutionManager(terminalServer);


var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
       
        /*
         * Sila Services
         */
        services.AddSingleton<IClientChannel>(terminalServer.Channel);
        services.AddSingleton<IClientExecutionManager>(terminalServerExecutionManager);
        services.AddSingleton<CashboxServiceClient>();
        services.AddSingleton<DisplayControllerClient>();
        services.AddSingleton<PrintingServiceClient>();
        services.AddSingleton<BarcodeScannerServiceClient>();
        services.AddSingleton<CardReaderServiceClient>();
        services.AddSingleton<IBankServer>(new BankServerClient(bankServer?.Channel, executionManagerFactory.CreateExecutionManager(bankServer)));

        services.AddSingleton<ICashDeskEvents, CashDeskEventPublisher>();
        services.AddSingleton<ICoordinatorEvents, CashDeskCoordinator>();
        services.AddHostedService<CashDeskEventHandler>();
        
        services.AddSingleton<ICashDesk, CashDesk.Application.CashDesk.CashDesk>();
        services.AddSingleton<IDisplayEventHandler, DisplayEventHandler>();
        services.AddSingleton<IPrinterEventHandler, PrinterEventHandler>();
        services.AddSingleton<IStoreGrpcService, StoreGrpcService>();
        services.AddSingleton<IBankService, BankService>();
        
        services.AddGrpcClient<StoreService.StoreServiceClient>(options =>
        {
            options.Address = new Uri("https://localhost:7228/grpc");
        }).ConfigurePrimaryHttpMessageHandler(() =>
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            // TODO
            // use LoadCertificate
            // -> handler.ClientCertificates.Add(LoadCertificate);
            return handler;
        });
    });

var app = builder.Build();

app.Run();