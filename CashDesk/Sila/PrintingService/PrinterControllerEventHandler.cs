using CashDesk.Classes.EventArgs;
using CashDesk.PrintingService;

namespace CashDesk.Sila.PrintingService;

/// <summary>
/// The event handler used for communicating with the printer
/// </summary>
public class PrinterControllerEventHandler
{
    private readonly ILogger _logger;
    private readonly PrintingServiceClient _printerClient;
    private double _runningTotal;

    /// <summary>
    /// This constructor initiates the logger, the <see cref="PrintingServiceClient"/>, the <see cref="CashDesk"/>
    /// and the <see cref="CashDeskEventPublisher"/>.
    /// </summary>
    /// <param name="logger">The logger object we want to log to</param>
    /// <param name="printerClient">The printer client we ultimately want to print on</param>
    /// <param name="cashDesk">The cashdesk we listen to the ChangeRunningTotal event</param>
    /// <param name="cdep">The Cashdesk event publisher we listen to the events <c>StartSale</c>
    /// and <c>FinishSale</c></param>
    public PrinterControllerEventHandler(
        ILogger<PrinterControllerEventHandler> logger, 
        PrintingServiceClient printerClient,
        CashDesk cashDesk,
        CashDeskEventPublisher cdep
    )
    {
        _logger = logger;
        _printerClient = printerClient;

        cdep.StartSale += StartSaleHandler;
        cdep.FinishSale += FinishSaleHandler;
        cashDesk.ChangeRunningTotal += ChangeRunningTotalHandler;
    }

    /// <summary>
    /// The handler method for the <c>StartSale</c> event.
    /// </summary>
    /// <param name="sender">The object this invocation originates from.</param>
    /// <param name="e">Should be empty</param>
    private void StartSaleHandler(object? sender, EventArgs e)
    {
        try
        {
            _printerClient.StartNext();
        }
        catch (Exception exception)
        {
            _logger.LogError("Printer could not start next receipt with reason {Reason}", exception.Message);
        }

    }
    
    /// <summary>
    /// The Handler for the <c>ChangeRunningTotal</c> event. It prints the newly added products name and it's price
    /// It also updates internally the current running total
    /// </summary>
    /// <param name="sender">The object this invocation originates from.</param>
    /// <param name="args">The args this event got invoked with, contains a product name, price and new running total </param>
    /// <seealso cref="ChangeRunningTotalArgs"/>
    private void ChangeRunningTotalHandler(object? sender, ChangeRunningTotalArgs args)
    {
        try
        {
            _printerClient.PrintLine($"{args.ProductName} \t {args.Price}");
            _runningTotal = args.Total;
        }
        catch (Exception exception)
        {
            _logger.LogError("Printer failed printing line with reason {Reason}", exception.Message);
        }

    }

    /// <summary>
    /// The Handler for the <c>FinishSale</c> event. Prints a spacer and the current total for this sale
    /// </summary>
    /// <param name="sender">The object this invocation originates from.</param>
    /// <param name="e">Should be empty</param>
    private void FinishSaleHandler(object? sender, EventArgs e)
    {
        try
        {
            _printerClient.PrintLine("__________________________________");
            _printerClient.PrintLine($"Total: \t \t \t {_runningTotal}");
        }
        catch (Exception exception)
        {
            _logger.LogError("Printer failed printing line with reason {Reason}", exception.Message);
        }
    }
    
    
}