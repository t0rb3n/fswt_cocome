using CashDesk.Domain.EventArgs;

namespace CashDesk.Application.Interfaces;

public interface ICashDesk
{

    event EventHandler<ChangeRunningTotalArgs>? ChangeRunningTotal;
    event EventHandler<SaleRegisteredArgs>? SaleRegistered;
    event EventHandler? SaleSuccess;
    event EventHandler<long>? ProductNotFound;
    event EventHandler<string>? BarcodeInvalid;
    
    /// <summary>
    /// Start point when the "Start Sale" Button was clicked. Checks that the state is legal and resets everything.
    /// </summary>
    void StartSale();
    
    /// <summary>
    /// Start Point when the "Finish Sale" button was clicked.
    /// Checks that the state is legal and sets correct state.
    /// </summary>
    void FinishSale();
    
    /// <summary>
    /// The Start point when a new barcode was scanned. 
    /// </summary>
    /// <param name="barcode">The unparsed barcode that was entered</param>
    void AddItemToSale(string barcode);
    
    /// <summary>
    /// Enables the expressmode
    /// </summary>
    void EnableExpressMode();
    
    /// <summary>
    /// Disables the express mode
    /// </summary>
    void DisableExpressMode();
    
    /// <summary>
    /// Startpoint of the "Pay with Cash" button. Checks state and calls <c>MakeSale</c>
    /// </summary>
    void PayWithCash();
    
    /// <summary>
    /// Startpoint of the "Pay with card" button. Checks state and express mode and tries to pay the
    /// sale by card and finish it by calling <c>MakeSale</c>
    /// </summary>
    void PayWithCard();

    /// <summary>
    /// Sets the state of the cashdesk to expect a card after one has failed.
    /// </summary>
    void PaymentModeRejected();
}