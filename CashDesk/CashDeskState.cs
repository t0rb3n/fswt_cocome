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