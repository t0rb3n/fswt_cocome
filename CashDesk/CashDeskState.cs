namespace CashDesk;

public enum CashDeskState
{
    /** Initial state. */
    ExpectingSale,

    /** After a sale has started ("New Sale" button pushed). */
    ExpectingItems,

    /**
	 * After a sale has finished (all products have been scanned) and
	 * "Finish Sale" button pushed.
	 */
    ExpectingPayment,

    /** After the choice of cash payment was made. */
    PayingByCash,

    /** After the cash payment. */
    PaidByCash,

    /** After the choice of credit card payment was made. */
    ExpectingCardInfo,

    /** After the credit card was scanned. */
    PayingByCreditCard
    
}

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