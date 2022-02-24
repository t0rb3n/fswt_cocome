using CashDesk.Domain.Enums;

namespace CashDesk.Domain.EventArgs;

/// <summary>
/// Used by the SaleRegistered event. Provides the necessary fields to handle a sale inside the coordinator
/// </summary>
public class SaleRegisteredArgs : System.EventArgs
{
    /// <summary>
    /// The amount of items that were sold
    /// </summary>
    public int Amount { get; init; }

    /// <summary>
    /// The <see cref="PaymentMode"/> used in this sale
    /// </summary>
    /// 
    public PaymentMode Mode { get; init; }
}