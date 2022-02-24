namespace CashDesk.Domain.Enums;

/// <summary>
/// The different possible states the cashdesk can be in
/// </summary>
public enum CashDeskState
{
    /// <summary> The Initial state of the cashdesk. Waiting that the button "Start sale" is clicked.</summary>
    ExpectingSale,

    /// <summary> After a sale has started ("New Sale" button pushed).</summary>
    ExpectingItems,

    /// <summary> After a sale has finished (all products have been scanned) and  "Finish Sale" button was pushed.</summary>
    ExpectingPayment,

    /// <summary> After the choice of cash payment was made.</summary>
    PayingByCash,
    
    /// <summary> After the choice of credit card payment was made..</summary>
    ExpectingCardInfo,

    /// <summary> After the credit card was scanned.</summary>
    PayingByCreditCard
}