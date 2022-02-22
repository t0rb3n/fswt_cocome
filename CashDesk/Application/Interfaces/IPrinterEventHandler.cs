using CashDesk.Domain.EventArgs;

namespace CashDesk.Application.Interfaces;

public interface IPrinterEventHandler
{
    
    /// <summary>
    /// The handler method for the <c>StartSale</c> event.
    /// </summary>
    /// <param name="sender">The object this invocation originates from.</param>
    /// <param name="e">Should be empty</param>
    void StartSaleHandler(object? sender, EventArgs e);
    
    /// <summary>
    /// The Handler for the <c>ChangeRunningTotal</c> event. It prints the newly added products name and it's price
    /// It also updates internally the current running total
    /// </summary>
    /// <param name="sender">The object this invocation originates from.</param>
    /// <param name="args">The args this event got invoked with, contains a product name, price and new running total </param>
    /// <seealso cref="ChangeRunningTotalArgs"/>
    void ChangeRunningTotalHandler(object? sender, ChangeRunningTotalArgs args);
    
    /// <summary>
    /// The Handler for the <c>FinishSale</c> event. Prints a spacer and the current total for this sale
    /// </summary>
    /// <param name="sender">The object this invocation originates from.</param>
    /// <param name="e">Should be empty</param>
    void FinishSaleHandler(object? sender, EventArgs e);
}