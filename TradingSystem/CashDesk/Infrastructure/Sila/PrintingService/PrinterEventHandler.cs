using CashDesk.Application.CashDesk;
using CashDesk.Application.Interfaces;
using CashDesk.Domain.EventArgs;
using CashDesk.PrintingService;

namespace CashDesk.Infrastructure.Sila.PrintingService;

/// <summary>
/// The event handler used for communicating with the printer
/// </summary>
public class PrinterEventHandler : IPrinterEventHandler
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
    public PrinterEventHandler(
        ILogger<PrinterEventHandler> logger, 
        PrintingServiceClient printerClient,
        ICashDesk cashDesk,
        ICashDeskEvents cdep
    )
    {
        _logger = logger;
        _printerClient = printerClient;

        cdep.StartSale += StartSaleHandler;
        cdep.FinishSale += FinishSaleHandler;
        cashDesk.ChangeRunningTotal += ChangeRunningTotalHandler;
    }
    
    public void StartSaleHandler(object? sender, EventArgs e)
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
    
    public void ChangeRunningTotalHandler(object? sender, ChangeRunningTotalArgs args)
    {
        try
        {
            _printerClient.PrintLine($"{args.ProductName} \t {args.Price} €");
            _runningTotal = args.Total;
        }
        catch (Exception exception)
        {
            _logger.LogError("Printer failed printing line with reason {Reason}", exception.Message);
        }

    }


    public void FinishSaleHandler(object? sender, EventArgs e)
    {
        try
        {
            _printerClient.PrintLine("__________________________________");
            _printerClient.PrintLine($"Total: \t \t \t {_runningTotal} €");
        }
        catch (Exception exception)
        {
            _logger.LogError("Printer failed printing line with reason {Reason}", exception.Message);
        }
    }
    
    
}