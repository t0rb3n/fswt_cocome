using CashDesk.Classes.EventArgs;
using CashDesk.DisplayController;

namespace CashDesk.Sila.DisplayController;

/// <summary>
/// This class is used to communicate with the DisplayController and handle the events raised by other classes
/// </summary>
public class DisplayControllerEventHandler
{
    private readonly ILogger _logger;
    /// <summary>
    /// The display we want to control and show text on.
    /// </summary>
    private DisplayControllerClient _displayClient;

    /// <summary>
    /// This constructor initiates the logger, the <see cref="CashDesk"/>, the <see cref="DisplayControllerClient"/>,
    /// the <see cref="CashDeskEventPublisher"/> and the <see cref="CashDeskCoordinator"/>.
    /// It also registers itself to be invoked upon most of the CashDesks events, some of the EventPublisher events and
    /// the coordinator events
    /// </summary>
    /// <param name="logger">The logger object we want to log to</param>
    /// <param name="cashDesk">The Cashdesk we are listening to</param>
    /// <param name="displayClient">The Display we want to control and show text on</param>
    /// <param name="cdep"> The CashDesk event publisher we want to listen to</param>
    /// <param name="cdc">The CashDeskCoordinator we want to listen to the events</param>
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

    /// <summary>
    /// The handler method for the <c>StartSale</c> event. 
    /// </summary>
    /// <param name="sender">The object this invocation originates from.</param>
    /// <param name="e">Should be empty</param>
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
    /// <summary>
    /// The handler method for the <c>ChangeRunningTotal</c> event. 
    /// </summary>
    /// <param name="sender">The object this invocation originates from.</param>
    /// <param name="args">The args used by this event, hold a product name, price and total.</param>
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

    /// <summary>
    /// The handler method for the <c>PayWithCard</c> event. 
    /// </summary>
    /// <param name="sender">The object this invocation originates from.</param>
    /// <param name="e">Should be empty</param>
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

    /// <summary>
    /// The handler method for the <c>SaleSuccess</c> event. 
    /// </summary>
    /// <param name="sender">The object this invocation originates from.</param>
    /// <param name="e">Should be empty</param>
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

    /// <summary>
    /// The handler method for the <c>PaymentModeRejected</c> event. 
    /// </summary>
    /// <param name="sender">The object this invocation originates from.</param>
    /// <param name="reason">The reason this paymentmode was declined.</param>
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
    
    /// <summary>
    /// The handler method for the <c>DisableExpressMode</c> event. 
    /// </summary>
    /// <param name="sender">The object this invocation originates from.</param>
    /// <param name="e">Should be empty</param>
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

    /// <summary>
    /// The handler method for the <c>EnableExpressMode</c> event. 
    /// </summary>
    /// <param name="sender">The object this invocation originates from.</param>
    /// <param name="e">Should be empty</param>
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

    /// <summary>
    /// The handler method for the <c>BarcodeInvalid</c> event. 
    /// </summary>
    /// <param name="sender">The object this invocation originates from.</param>
    /// <param name="barcode">The barcode that was declined.</param>
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
    
    /// <summary>
    /// The handler method for the <c>ProductNotFound</c> event. 
    /// </summary>
    /// <param name="sender">The object this invocation originates from.</param>
    /// <param name="barcode">The barcode to the product that wasn't found.</param>
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