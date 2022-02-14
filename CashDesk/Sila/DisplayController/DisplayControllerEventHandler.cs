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
        cdep.StartSale += StartSaleHandler;
        cdep.PayWithCard += PayWithCardHandler;
        cdep.DisableExpressMode += DisableExpressModeHandler;
        cdc.EnableExpressMode += EnableExpressModeHandler;

        //cdep.AddItemToSale += AddItemToSaleHandler;


    }
    
    private void StartSaleHandler(object sender, EventArgs e)
    {
        _displayClient.SetDisplayText("New Sale");
    }

    private void ChangeRunningTotalHandler(object sender, ChangeRunningTotalArgs args)
    {
        _displayClient.SetDisplayText($"{args.ProductName}: {args.Price}\nTotal: {args.Total} ");
    }

    private void PayWithCardHandler(object sender, EventArgs e)
    {
        _displayClient.SetDisplayText("Waiting for the card...");
    }

    private void SaleSuccessHandler(object sender, EventArgs e)
    {
        _displayClient.SetDisplayText("Thank you for shopping with us. Goodbye");
    }
    private void PaymentModeRejectedHandler(object sender, string reason)
    {
        _displayClient.SetDisplayText(reason);
    }

    private void DisableExpressModeHandler(object sender, EventArgs e)
    {
        _displayClient.SetDisplayText("Express mode disabled.");
    }
    
    private void EnableExpressModeHandler(object sender, EventArgs e)
    {
        _displayClient.SetDisplayText("Express mode enabled.");
    }
}


public class ChangeRunningTotalArgs : EventArgs
{
    public string ProductName { get; set; }
    public double Price { get; set; }
    public double Total { get; set; }
}