using CashDesk.Classes;
using CashDesk.Classes.EventArgs;
using CashDesk.DisplayController;

namespace CashDesk.Sila.DisplayController;

public class DisplayControllerEventHandler
{
    private readonly ILogger _logger;
    private DisplayControllerClient _displayClient;

    public DisplayControllerEventHandler(
        ILogger<DisplayControllerEventHandler> logger,
        CashDesk cashDesk,
        DisplayControllerClient displayClient,
        CashDeskEventPublisher cdep,
        CashDeskCoordinator cdc
    )
    {
        _logger = logger;
        _displayClient = displayClient;

        cashDesk.ChangeRunningTotal += ChangeRunningTotalHandler;
        cashDesk.SaleSuccess += SaleSuccessHandler;
        cashDesk.PaymentModeRejected += PaymentModeRejectedHandler;
        cashDesk.BarcodeInvalid += BarcodeInvalidHandler;
        cashDesk.ProductNotFound += ProductNotFoundHandler;
        
        cdep.StartSale += StartSaleHandler;
        cdep.PayWithCard += PayWithCardHandler;
        cdep.DisableExpressMode += DisableExpressModeHandler;
        
        cdc.EnableExpressMode += EnableExpressModeHandler;
    }

    private void StartSaleHandler(object? sender, EventArgs e)
    {
        try
        {
            _displayClient.SetDisplayText("New Sale");
        }
        catch (Exception exception)
        {
            _logger.LogError("Could not communicate with the display with reason {Reason}", exception.Message);
        }
    }

    private void ChangeRunningTotalHandler(object? sender, ChangeRunningTotalArgs args)
    {
        try
        {
            _displayClient.SetDisplayText($"{args.ProductName}: {args.Price}\nTotal: {args.Total} ");
        }
        catch (Exception exception)
        {
            _logger.LogError("Could not communicate with the display with reason {Reason}", exception.Message);
        }
    }

    private void PayWithCardHandler(object? sender, EventArgs e)
    {
        try
        {
            _displayClient.SetDisplayText("Waiting for the card...");
        }
        catch (Exception exception)
        {
            _logger.LogError("Could not communicate with the display with reason {Reason}", exception.Message);
        }
    }

    private void SaleSuccessHandler(object? sender, EventArgs e)
    {
        try
        {
            _displayClient.SetDisplayText("Thank you for shopping with us. Goodbye");
        }
        catch (Exception exception)
        {
            _logger.LogError("Could not communicate with the display with reason {Reason}", exception.Message);
        }
    }

    private void PaymentModeRejectedHandler(object? sender, string reason)
    {
        try
        {
            _displayClient.SetDisplayText(reason);
        }
        catch (Exception exception)
        {
            _logger.LogError("Could not communicate with the display with reason {Reason}", exception.Message);
        }
    }

    private void DisableExpressModeHandler(object? sender, EventArgs e)
    {
        try
        {
            _displayClient.SetDisplayText("Express mode disabled.");
        }
        catch (Exception exception)
        {
            _logger.LogError("Could not communicate with the display with reason {Reason}", exception.Message);
        }
    }

    private void EnableExpressModeHandler(object? sender, EventArgs e)
    {
        try
        {
            _displayClient.SetDisplayText("Express mode enabled.");
        }
        catch (Exception exception)
        {
            _logger.LogError("Could not communicate with the display with reason {Reason}", exception.Message);
        }
    }

    private void BarcodeInvalidHandler(object? sender, string barcode)
    {
        try
        {
            _displayClient.SetDisplayText($"Barcode {barcode} has to be a number");
        }
        catch (Exception exception)
        {
            _logger.LogError("Could not communicate with the display with reason {Reason}", exception.Message);
        }
    }
    
    private void ProductNotFoundHandler(object? sender, long barcode)
    {
        try
        {
            _displayClient.SetDisplayText($"Product with {barcode} does not exist in this store.");
        }
        catch (Exception exception)
        {
            _logger.LogError("Could not communicate with the display with reason {Reason}", exception.Message);
        }
    }
}