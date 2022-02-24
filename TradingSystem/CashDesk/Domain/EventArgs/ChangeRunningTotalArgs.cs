namespace CashDesk.Domain.EventArgs;

/// <summary>
/// The arguments needed for the ChangeRunningTotal event
/// </summary>
public class ChangeRunningTotalArgs : System.EventArgs
{
    /// <value>
    /// The name of the product
    /// </value>
    public string ProductName { get; init; }
    
    /// <value>
    /// The price of the product
    /// </value>
    public double Price { get; init; }
    
    /// <value>
    /// The new current total after this product was added.
    /// </value>
    public double Total { get; init; }
}