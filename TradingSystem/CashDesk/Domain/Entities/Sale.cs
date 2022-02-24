using CashDesk.Domain.EventArgs;

namespace CashDesk.Domain.Entities;

/// <summary>
/// A wrapper class to be used to inside of the coordinator to evaluate if
/// express mode for this cashdesk is needed or not
/// </summary>
public class Sale : SaleRegisteredArgs
{
    /// <value>
    /// The date and time this sale was made
    /// </value>
    public DateTime Date { get; init; }
}