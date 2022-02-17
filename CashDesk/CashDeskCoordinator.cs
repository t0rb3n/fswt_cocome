using CashDesk.Classes;
using CashDesk.Classes.Enums;
using CashDesk.Classes.EventArgs;

namespace CashDesk;

public class CashDeskCoordinator
{
    public event EventHandler? EnableExpressMode;
    private readonly ILogger<CashDeskCoordinator> _logger;
    private DateTime _lastCheck;
    private readonly Queue<Sale> _history = new Queue<Sale>();
    private bool _expressModeNeeded;

    public CashDeskCoordinator(
        ILogger<CashDeskCoordinator> logger,
        CashDesk cashDesk
    )
    {
        _logger = logger;
        cashDesk.SaleRegistered += SaleRegisteredHandler;
    }

    private void SaleRegisteredHandler(object? sender, SaleRegisteredArgs args)
    {
        var sale = new Sale
        {
            Date = DateTime.Now,
            Amount = args.Amount,
            Mode = args.Mode,
        };
        _history.Enqueue(sale);
        CleanHistory();

        if (!IsExpressModeNeeded()) return;

        _logger.LogInformation("Enabling Express mode for cash-desk");
        EnableExpressMode?.Invoke(this, EventArgs.Empty);
    }

    // Clean the sale history and removes every item its age is greater than the policy allows
    private void CleanHistory()
    {
        var now = DateTime.Now;
        //TODO care for Peek() exception
        while ((_history.Peek().Date - now).TotalMinutes >= ExpressModePolicy.SalesWindow)
        {
            _history.Dequeue();
        }
    }


    private bool IsExpressModeNeeded()
    {
        var now = DateTime.Now;

        if (!IsCheckNeeded(now)) return _expressModeNeeded;

        CleanHistory();
        _lastCheck = now;
        _expressModeNeeded = EvaluateExpressMode(_history);

        return _expressModeNeeded;
    }

    // Checks if the time since the last check is greater than the policy allows
    private bool IsCheckNeeded(DateTime now)
    {
        return (now - _lastCheck).TotalSeconds >= ExpressModePolicy.CheckPeriodSeconds;
    }

    // checks if the ratio of sales that count as express sales is greater than the policy allows
    private static bool EvaluateExpressMode(IReadOnlyCollection<Sale> history)
    {
        var allSalesCount = history.Count;
        var expressSalesCount = CountEligibleExpressSales(history);

        var ratio = (double) expressSalesCount / allSalesCount;

        return ratio >= ExpressModePolicy.ExpressThreshold;
    }

    // checks the sales and counts how many count as express sales Eligible
    private static int CountEligibleExpressSales(IEnumerable<Sale> history)
    {
        return history.Sum(sale =>
            (sale.Mode == PaymentMode.Cash && sale.Amount <= ExpressModePolicy.ExpressItemsLimit)
                ? 1
                : 0);
    }
}