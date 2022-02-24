using CashDesk.Domain.EventArgs;

namespace CashDesk.Application.Interfaces;


/// <summary>
/// The interface for the Class to listen to events and show text at the Display accordingly
/// </summary>
public interface IDisplayEventHandler
{
    /// <summary>
    /// The handler method for the <c>StartSale</c> event. 
    /// </summary>
    /// <param name="sender">The object this invocation originates from.</param>
    /// <param name="e">Should be empty</param>
    void StartSaleHandler(object? sender, EventArgs e);
    
    /// <summary>
    /// The handler method for the <c>ChangeRunningTotal</c> event. 
    /// </summary>
    /// <param name="sender">The object this invocation originates from.</param>
    /// <param name="args">The args used by this event, hold a product name, price and total.</param>
    void ChangeRunningTotalHandler(object? sender, ChangeRunningTotalArgs args);
    
    /// <summary>
    /// The handler method for the <c>PayWithCard</c> event. 
    /// </summary>
    /// <param name="sender">The object this invocation originates from.</param>
    /// <param name="e">Should be empty</param>
    void PayWithCardHandler(object? sender, EventArgs e);
    
    /// <summary>
    /// The handler method for the <c>SaleSuccess</c> event. 
    /// </summary>
    /// <param name="sender">The object this invocation originates from.</param>
    /// <param name="e">Should be empty</param> 
    void SaleSuccessHandler(object? sender, EventArgs e);
    
    /// <summary>
    /// The handler method for the <c>PaymentModeRejected</c> event. 
    /// </summary>
    /// <param name="sender">The object this invocation originates from.</param>
    /// <param name="reason">The reason this paymentmode was declined.</param>
    void PaymentModeRejectedHandler(object? sender, string reason);
    
    /// <summary>
    /// The handler method for the <c>DisableExpressMode</c> event. 
    /// </summary>
    /// <param name="sender">The object this invocation originates from.</param>
    /// <param name="e">Should be empty</param>
    void DisableExpressModeHandler(object? sender, EventArgs e);

    /// <summary>
    /// The handler method for the <c>EnableExpressMode</c> event. 
    /// </summary>
    /// <param name="sender">The object this invocation originates from.</param>
    /// <param name="e">Should be empty</param>
    void EnableExpressModeHandler(object? sender, EventArgs e);

    /// <summary>
    /// The handler method for the <c>BarcodeInvalid</c> event. 
    /// </summary>
    /// <param name="sender">The object this invocation originates from.</param>
    /// <param name="barcode">The barcode that was declined.</param>
    void BarcodeInvalidHandler(object? sender, string barcode);
    
    /// <summary>
    /// The handler method for the <c>ProductNotFound</c> event. 
    /// </summary>
    /// <param name="sender">The object this invocation originates from.</param>
    /// <param name="barcode">The barcode to the product that wasn't found.</param>
    void ProductNotFoundHandler(object? sender, long barcode);
    /// <summary>
    /// The handler method for the <c>OutOfStock</c> event. 
    /// </summary>
    /// <param name="sender">The object this invocation originates from.</param>
    /// <param name="productName">The name of the product that does not exist in this store</param>
    void OutOfStockHandler(object? sender, string productName);
}