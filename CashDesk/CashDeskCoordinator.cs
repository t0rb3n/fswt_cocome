namespace CashDesk;

public class CashDeskCoordinator
{
    public event EventHandler EnableExpressMode;
    private readonly ILogger<CashDeskCoordinator> _logger;
    private DateTime _lastCheck;
    private Queue<Sale> History = new Queue<Sale>();
    private bool _expressModeNeeded = false;

    public CashDeskCoordinator(
        ILogger<CashDeskCoordinator> logger,
        CashDesk cashDesk
    )
    {
        cashDesk.SaleRegistered += SaleRegisteredHandler;
    }

    private void SaleRegisteredHandler(object sender, SaleRegisteredArgs args)
    {
        var sale = new Sale
        {
            Date = DateTime.Now,
            Amount = args.Amount,
            Mode = args.Mode,
        };
        History.Enqueue(sale);
        CleanHistory();

        if (IsExpressModeNeeded())
        {
            EnableExpressMode?.Invoke(this, EventArgs.Empty);
        }
    }
    
    // Clean the sale history and removes every item its age is greater than the policy allows
    private void CleanHistory()
    {
        DateTime now = DateTime.Now;
            //TODO care for Peek() exception
        while ((History.Peek().Date - now).TotalMinutes >= ExpressModePolicy._salesWindow)
        {
            History.Dequeue();
        }
    }


    private bool IsExpressModeNeeded()
    {
        var now = DateTime.Now;

        if (IsCheckNeeded(now))
        {
            CleanHistory();
            _lastCheck = now;
            _expressModeNeeded = EvaluateExpressMode(History);
        }

        return _expressModeNeeded;
    }

    // Checks if the time since the last check is greater than the policy allows
    private bool IsCheckNeeded(DateTime now)
    {
        return (now - _lastCheck).TotalSeconds >= ExpressModePolicy._checkPeriodSeconds;
    }

    // checks if the ratio of sales that count as expresssales is greater than the policy allows
    private bool EvaluateExpressMode(Queue<Sale> history)
    {
        var allSalesCount = history.Count;
        var expressSalesCount = CountEligbleExpressSales(history);

        double ratio = (double) expressSalesCount / allSalesCount;

        return ratio >= ExpressModePolicy._expressThreshold;
    }

    // checks the sales and counts how many count as express sales
    private int CountEligbleExpressSales(Queue<Sale> history)
    {
        int counter = 0;
        foreach (var sale in history)
        {
            counter +=
                (sale.Mode == PaymentMode.Cash
                 && sale.Amount <= ExpressModePolicy._expressItemsLimit)
                    ? 1
                    : 0;
        }

        return counter;
    }
}

public class Sale : SaleRegisteredArgs
{
    public DateTime Date { get; set; }
}