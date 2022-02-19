using CashDesk.Sila.DisplayController;
using CashDesk.Sila.PrintingService;

namespace CashDesk;

/// <summary>
/// This EventHandler is used to listen most of the events produced by the <see cref="CashDeskEventPublisher"/>
/// and to provide a startup method for this application.
/// </summary>
public class CashDeskEventHandler : IHostedService
{
    private readonly CashDeskEventPublisher _cdep;
    private readonly CashDesk _cashDesk;

    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _appLifetime;

    /// <summary> This constructor initalizes the logger, the <see cref="CashDesk"/>, the <see cref="CashDeskEventPublisher"/>,
    /// the <see cref="CashDeskCoordinator"/>, the <see cref="DisplayControllerEventHandler"/> and <see cref="PrinterControllerEventHandler"/>.
    /// It also registers itself to most of the EventPublishers events.</summary>
    /// <param name="logger">The logger object to into</param>
    /// <param name="cashDesk"> The cashdesk we want to listen to</param>
    /// <param name="appLifetime"> Applifetime used by .NET</param>
    /// <param name="cdep"> The Cashdesk event publisher which events we want to handle</param>
    /// <param name="cdc"> The coordinator we want to listen to the EnableExpressMode event</param>
    /// <param name="dceh"> This is just there so that .NET loads these singletons and they start listening to events</param>
    /// <param name="pceh"> This is just there so that .NET loads these singletons and they start listening to events</param>
    public CashDeskEventHandler(
        ILogger<CashDeskEventHandler> logger,
        IHostApplicationLifetime appLifetime,
        CashDesk cashDesk,
        CashDeskEventPublisher cdep,
        CashDeskCoordinator cdc,
        DisplayControllerEventHandler dceh,
        PrinterControllerEventHandler pceh)
    {
        _logger = logger;
        _appLifetime = appLifetime;
        _cashDesk = cashDesk;
        _cdep = cdep;

        cdc.EnableExpressMode += EnableExpressModeHandler;

        RegisterCashDeskHandler();
    }

    /// <summary> Helper method to not clutter the constructor. Registers methods to be invoked upon an event. </summary>
    private void RegisterCashDeskHandler()
    {
        _cdep.StartSale += StartSaleHandler;
        _cdep.FinishSale += FinishSaleHandler;
        _cdep.DisableExpressMode += DisableExpressModeHandler;
        _cdep.PayWithCard += PayWithCardHandler;
        _cdep.PayWithCash += PayWithCashHandler;

        _cdep.AddItemToSale += AddItemToSaleHandler;
    }

    /*
     * Hosted Service Methods
     */

    /// <summary>
    ///  Method provided by <see cref="IHostedService"/> interface. Upon running it starts listening to the cashbox buttons
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns> Nothing</returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _appLifetime.ApplicationStarted.Register(() =>
        {
            Task.Run(async () =>
            {
                try
                {
                    await _cdep.StartListeningToTerminal();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unhandled exception!");
                }
                finally
                {
                    {
                        _appLifetime.StopApplication();
                    }
                }
            });
        });

        return Task.CompletedTask;
    }

    /// <summary>
    ///  Method provided by <see cref="IHostedService"/> interface. Upon running it stops the application.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>Nothing</returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /*
     *  Handler Methods
     */

    /// <summary>
    /// The Handler method to listen to the StartSale Event
    /// <seealso cref="CashDeskEventPublisher"/>
    /// </summary>
    /// <param name="sender">The object this invocation originates from</param>
    /// <param name="e"> The event args which should be empty</param>
    private void StartSaleHandler(object? sender, EventArgs e)
    {
        _cashDesk.StartSale();
    }

    /// <summary>
    /// The Handler method to listen to the AddItemToSale Event
    /// </summary>
    /// <param name="sender">The object this invocation originates from</param>
    /// <param name="barcode">The barcode that got scanned</param>
    private void AddItemToSaleHandler(object? sender, string barcode)
    {
        _cashDesk.AddItemToSale(barcode);
    }

    /// <summary>
    /// The Handler method to listen to the FinishSale Event
    /// </summary>
    /// <param name="sender">The object this invocation originates from</param>
    /// <param name="e"> The event args which should be empty</param>
    private void FinishSaleHandler(object? sender, EventArgs e)
    {
        _cashDesk.FinishSale();
    }

    /// <summary>
    /// The Handler method to listen to the PayWithCard Event
    /// </summary>
    /// <param name="sender">The object this invocation originates from</param>
    /// <param name="e"> The event args which should be empty</param>
    private void PayWithCardHandler(object? sender, EventArgs e)
    {
        _cashDesk.PayWithCard();
    }

    /// <summary>
    /// The Handler method to listen to the PayWithCash Event
    /// </summary>
    /// <param name="sender">The object this invocation originates from</param>
    /// <param name="e"> The event args which should be empty</param>
    private void PayWithCashHandler(object? sender, EventArgs e)
    {
        _cashDesk.PayWithCash();
    }

    /// <summary>
    /// The Handler method to listen to the EnableExpressMode Event
    /// </summary>
    /// <param name="sender">The object this invocation originates from</param>
    /// <param name="e"> The event args which should be empty</param>
    private void EnableExpressModeHandler(object? sender, EventArgs e)
    {
        _cashDesk.EnableExpressMode();
    }

    /// <summary>
    /// The Handler method to listen to the DisableExpressMode Event
    /// </summary>
    /// <param name="sender">The object this invocation originates from</param>
    /// <param name="e"> The event args which should be empty</param>
    private void DisableExpressModeHandler(object? sender, EventArgs e)
    {
        _cashDesk.DisableExpressMode();
    }
}