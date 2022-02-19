using CashDesk.Classes;
using CashDesk.Classes.Enums;
using CashDesk.Classes.EventArgs;

namespace CashDesk;

/// <summary>
///  This class is used to determine if a cashDesk needs express mode or not
/// </summary>
public class CashDeskCoordinator
{
    public event EventHandler? EnableExpressMode;
    
    private readonly ILogger<CashDeskCoordinator> _logger;
    private readonly Queue<Sale> _history = new Queue<Sale>();

    private DateTime _lastCheck;
    private bool _expressModeNeeded;

    
    /// <summary> This constructor initalizes the logger and the Cashdesk
    /// that this Coordinator is listening to. It also registers itself to the SaleRegisteredEvent</summary>
    /// <param name="logger">The logger object to into</param>
    /// <param name="cashDesk"> The cashdesk we want to coordinate and listen to</param>
    public CashDeskCoordinator(
        ILogger<CashDeskCoordinator> logger,
        CashDesk cashDesk
    )
    {
        _logger = logger;
        cashDesk.SaleRegistered += SaleRegisteredHandler;
    }

    /// <summary> The Method handler for the SaleRegistered event </summary>
    /// <param name="sender"> The object this invocation originates from</param>
    /// <param name="args"> The arguments passed to the event</param>
    /// <see cref="SaleRegisteredArgs"/>
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

    /// <summary> Clean the sale history and removes every item its age is greater than the policy allows </summary>
    private void CleanHistory()
    {
        var now = DateTime.Now;
        while ((_history.Peek().Date - now).TotalMinutes >= ExpressModePolicy.SalesWindow)
        {
            _history.Dequeue();
        }
    }
    
    /// <summary> Higher up method to evalute express mode </summary>
    /// <returns> True if express Mode is needed; otherwise, false</returns>
    private bool IsExpressModeNeeded()
    {
        var now = DateTime.Now;

        if (!IsCheckNeeded(now)) return _expressModeNeeded;

        CleanHistory();
        _lastCheck = now;
        _expressModeNeeded = EvaluateExpressMode(_history);

        return _expressModeNeeded;
    }

    /// <summary> Checks if the time since the last check is greater than the policy allows </summary>
    /// <returns> True if last check exceed poliys limit; otherwise, false</returns>
    private bool IsCheckNeeded(DateTime now)
    {
        return (now - _lastCheck).TotalSeconds >= ExpressModePolicy.CheckPeriodSeconds;
    }


    /// <summary> Checks if the ratio of sales that count as express sales is greater than the policy allows </summary>
    /// <param name="history"> A list of sales </param>
    /// <returns> True if express mode is needed ; otherwise, false</returns>
    private static bool EvaluateExpressMode(IReadOnlyCollection<Sale> history)
    {
        var allSalesCount = history.Count;
        var expressSalesCount = CountEligibleExpressSales(history);

        var ratio = (double) expressSalesCount / allSalesCount;

        return ratio >= ExpressModePolicy.ExpressThreshold;
    }


    /// <summary> Checks the sales and counts how many count as express sales </summary>
    /// <param name="history"> A list of sales </param>
    /// <returns> The amount of sales that count as express sales</returns>
    private static int CountEligibleExpressSales(IEnumerable<Sale> history)
    {
        return history.Sum(sale =>
            (sale.Mode == PaymentMode.Cash && sale.Amount <= ExpressModePolicy.ExpressItemsLimit)
                ? 1
                : 0);
    }
}