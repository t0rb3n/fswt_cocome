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
        CashDeskEventPublisher cdep
    )
    {
        _logger = logger;
        _displayClient = displayClient;
        
        cashDesk.UpdateDisplay += UpdateDisplayHandler;
        cashDesk.ChangeRunningTotal += ChangeRunningTotalHandler;
        cdep.StartSale += StartSaleHandler;
        //cdep.AddItemToSale += AddItemToSaleHandler;
        

    }

    private void UpdateDisplayHandler(object sender, string text)
    {
        _displayClient.SetDisplayText(text);
    }

    private void StartSaleHandler(object sender, EventArgs e)
    {
        _displayClient.SetDisplayText("New Sale");
    }

    private void ChangeRunningTotalHandler(object sender, ChangeRunningTotalArgs args)
    {
        _displayClient.SetDisplayText($"{args.ProductName}: {args.Price}\n Total: {args.Total} ");
    }
}


public class ChangeRunningTotalArgs : EventArgs
{
    public string ProductName { get; set; }
    public double Price { get; set; }
    public double Total { get; set; }
}