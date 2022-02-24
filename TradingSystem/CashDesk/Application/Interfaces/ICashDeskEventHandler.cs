using CashDesk.Application.CashDesk;

namespace CashDesk.Application.Interfaces;

/// <summary>
/// The Interface necessary to handle the CashBox buttons and Barcode scanner events.
/// </summary>
public interface ICashDeskEventHandler
{
    
    /// <summary>
    /// The Handler method to listen to the StartSale Event
    /// <seealso cref="CashDeskEventPublisher"/>
    /// </summary>
    /// <param name="sender">The object this invocation originates from</param>
    /// <param name="e"> The event args which should be empty</param>
    void StartSaleHandler(object? sender, EventArgs e);
    /// <summary>
    /// The Handler method to listen to the AddItemToSale Event
    /// </summary>
    /// <param name="sender">The object this invocation originates from</param>
    /// <param name="barcode">The barcode that got scanned</param>
    void AddItemToSaleHandler(object? sender, string barcode);
    /// <summary>
    /// The Handler method to listen to the FinishSale Event
    /// </summary>
    /// <param name="sender">The object this invocation originates from</param>
    /// <param name="e"> The event args which should be empty</param>
    void FinishSaleHandler(object? sender, EventArgs e);
    /// <summary>
    /// The Handler method to listen to the PayWithCard Event
    /// </summary>
    /// <param name="sender">The object this invocation originates from</param>
    /// <param name="e"> The event args which should be empty</param>
    void PayWithCardHandler(object? sender, EventArgs e);
    /// <summary>
    /// The Handler method to listen to the PayWithCash Event
    /// </summary>
    /// <param name="sender">The object this invocation originates from</param>
    /// <param name="e"> The event args which should be empty</param>
    void PayWithCashHandler(object? sender, EventArgs e);
    
    /// <summary>
    /// The Handler method to listen to the EnableExpressMode Event
    /// </summary>
    /// <param name="sender">The object this invocation originates from</param>
    /// <param name="e"> The event args which should be empty</param>
    void EnableExpressModeHandler(object? sender, EventArgs e);
    
    /// <summary>
    /// The Handler method to listen to the DisableExpressMode Event
    /// </summary>
    /// <param name="sender">The object this invocation originates from</param>
    /// <param name="e"> The event args which should be empty</param>
    void DisableExpressModeHandler(object? sender, EventArgs e);

    /// <summary>
    /// The handler method to listen to the PaymentModeRejected Event from <see cref="IBankService"/>
    /// </summary>
    /// <param name="sender">The object this invocation originates from</param>
    /// <param name="e"> The reason this payment mode got rejected</param>
    void PaymentModeRejectedHandler(object sender, string reason);
}