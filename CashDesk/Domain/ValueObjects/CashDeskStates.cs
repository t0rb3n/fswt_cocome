using CashDesk.Domain.Enums;

namespace CashDesk.Domain.ValueObjects;

/// <summary>
/// The states the cashdesk is allowed to be in when a certain event is invoked. The event is defined by the
/// prefix of the member: <c>StartSaleStates</c> maps to the StartSale event
/// </summary>
public abstract class CashDeskStates
{
    /// <summary>
    /// New sale can be started anytime and thus aborted expect when we already paid by cash 
    /// </summary>
    public static readonly HashSet<CashDeskState> StartSaleStates = new HashSet<CashDeskState>
    {
        CashDeskState.ExpectingSale,
        CashDeskState.ExpectingItems,
        CashDeskState.ExpectingPayment,
        CashDeskState.PayingByCash,
        CashDeskState.ExpectingCardInfo,
        CashDeskState.PayingByCreditCard
    };

    public static readonly HashSet<CashDeskState> AddItemToSaleStates = new HashSet<CashDeskState>
    {
        CashDeskState.ExpectingItems
    };

    public static readonly HashSet<CashDeskState> FinishSaleStates = new HashSet<CashDeskState>
    {
        CashDeskState.ExpectingItems
    };

    public static readonly HashSet<CashDeskState> SelectPayingModeStates = new HashSet<CashDeskState>
    {
        CashDeskState.ExpectingPayment,
        CashDeskState.ExpectingCardInfo,
        CashDeskState.PayingByCash
    };
}