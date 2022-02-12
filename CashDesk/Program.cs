using System.Net.NetworkInformation;
using CashDesk;
using CashDesk.BankServer;
using CashDesk.BarcodeScannerService;
using CashDesk.CardReaderService;
using CashDesk.CashboxService;
using CashDesk.DisplayController;
using CashDesk.PrintingService;
using Grpc.Core;
using GrpcModule.Services.Enterprise;
using Tecan.Sila2;
using Tecan.Sila2.Client;
using Tecan.Sila2.Client.ExecutionManagement;
using Tecan.Sila2.Discovery;


var connector = new ServerConnector(new DiscoveryExecutionManager());
var discovery = new ServerDiscovery(connector);
var servers = discovery.GetServers(TimeSpan.FromSeconds(10),
    n => n.NetworkInterfaceType == NetworkInterfaceType.Loopback);
var terminalServer = servers.First(s => s.Info.Type == "Terminal");
var bankServer = servers.FirstOrDefault(s => s.Info.Type == "BankServer");
var executionManagerFactory = new ExecutionManagerFactory(Enumerable.Empty<IClientRequestInterceptor>());
var terminalServerExecutionManager = executionManagerFactory.CreateExecutionManager(terminalServer);


var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IClientChannel>(terminalServer.Channel);
        services.AddSingleton<IClientExecutionManager>(terminalServerExecutionManager);
        services.AddSingleton<CashboxServiceClient>();
        services.AddSingleton<DisplayControllerClient>();
        services.AddSingleton<PrintingServiceClient>();
        services.AddSingleton<BarcodeScannerServiceClient>();
        services.AddSingleton<CardReaderServiceClient>();
        services.AddSingleton<IBankServer>(new BankServerClient(bankServer?.Channel, executionManagerFactory.CreateExecutionManager(bankServer)));
        
        services.AddSingleton<CashDeskEventPublisher>();
        services.AddSingleton<CashDesk.CashDesk>();
        services.AddHostedService<CashDeskEventHandler>();
        
        services.AddGrpcClient<EnterpriseService.EnterpriseServiceClient>(options =>
        {
            options.Address = new Uri("https://localhost:7046/grpc");
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