using CashDesk.PrintingService;
using CashDesk.Sila.DisplayController;

namespace CashDesk.Sila.PrintingService;

public class PrinterControllerEventHandler
{
    private readonly ILogger _logger;
    private PrintingServiceClient _printerClient;
    private double runningTotal;

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

    private void StartSaleHandler(object sender, EventArgs e)
    {
        _printerClient.StartNext();

    }
    private void ChangeRunningTotalHandler(object sender, ChangeRunningTotalArgs args)
    {
        _printerClient.PrintLine($"{args.ProductName} \t {args.Price}");
        runningTotal = args.Total;
    }

    private void FinishSaleHandler(object sender, EventArgs e)
    {
        // TODO if fancy try to make it that the price is always at the same spot
        // also make the limiter the right length
        _printerClient.PrintLine("__________________________________");
        _printerClient.PrintLine($"Total: \t \t \t {runningTotal}");
    }
    
    
}