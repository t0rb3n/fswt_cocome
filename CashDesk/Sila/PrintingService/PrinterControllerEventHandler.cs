using CashDesk.Classes;
using CashDesk.Classes.EventArgs;
using CashDesk.PrintingService;
using CashDesk.Sila.DisplayController;

namespace CashDesk.Sila.PrintingService;

public class PrinterControllerEventHandler
{
    private readonly ILogger _logger;
    private readonly PrintingServiceClient _printerClient;
    private double _runningTotal;

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