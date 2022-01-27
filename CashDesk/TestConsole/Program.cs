// See https://aka.ms/new-console-template for more information

using System.Net.NetworkInformation;
using CashDesk;
using CashDesk.CashboxService;
using CashDesk.DisplayController;
using Tecan.Sila2;
using Tecan.Sila2.Client;
using Tecan.Sila2.Client.ExecutionManagement;
using Tecan.Sila2.Discovery;

Console.WriteLine("Hello, World!");
var connector = new ServerConnector(new DiscoveryExecutionManager());
var discovery = new ServerDiscovery(connector);
var executionManagerFactory = new ExecutionManagerFactory(Enumerable.Empty<IClientRequestInterceptor>());

var servers = discovery.GetServers(TimeSpan.FromSeconds(10), n => n.NetworkInterfaceType == NetworkInterfaceType.Loopback);
ServerData terminalServer;
try
{
    terminalServer = servers.First(s => s.Info.Type == "Terminal");
}
catch (InvalidOperationException e)
{
    throw new NoSuchServiceException("No Terminalserver could be found! Make sure it's running.");
}
var bankServer = servers.FirstOrDefault(s => s.Info.Type == "BankServer") ?? throw new NoSuchServiceException("No Bankserver could be found! Make sure it's running.");

Console.WriteLine(terminalServer);
var terminalServerExecutionManager = executionManagerFactory.CreateExecutionManager(terminalServer);

// demo of cashbox and display
var cashboxClient = new CashboxServiceClient(terminalServer.Channel, terminalServerExecutionManager);
var displayClient = new DisplayControllerClient(terminalServer.Channel, terminalServerExecutionManager);
var cashboxButtons = cashboxClient.ListenToCashdeskButtons();
Console.WriteLine("Reading cashbox buttons");
Console.WriteLine("Press some of the cashbox buttons and see how the display adjust. Press Enter to continue");
var listenToCashBoxButtons = cashboxClient.ListenToCashdeskButtons();
DisplayButtonsPressed(displayClient, listenToCashBoxButtons);
Console.ReadLine();
listenToCashBoxButtons.Cancel();










static async void DisplayButtonsPressed(IDisplayController display, IIntermediateObservableCommand<CashboxButton> cashboxButtons)
{
    while (await cashboxButtons.IntermediateValues.WaitToReadAsync())
    {
        if (cashboxButtons.IntermediateValues.TryRead(out var button))
        {
            display.SetDisplayText($"{button} pressed");
        }
    }
}