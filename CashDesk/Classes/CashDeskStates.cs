

using CashDesk.Classes.Enums;

namespace CashDesk.Classes;

public abstract class CashDeskStates
{
    // New sale can be started anytime and thus aborted expect when we already paid by cash 
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