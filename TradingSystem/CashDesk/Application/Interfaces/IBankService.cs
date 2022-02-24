namespace CashDesk.Application.Interfaces;

public interface IBankService
{
    event EventHandler<string>? PaymentModeRejected;
    
    /// <summary>
    /// This method tries to validate the card let the customer pay by card.
    /// </summary>
    /// <param name="amount">The amount that has to be paid</param>
    Task TryPayingByCard(long amount);

    /// <summary>
    /// Invokes the <c>PaymentModeRejected</c> Event
    /// </summary>
    /// <param name="reason">The reason this payment was rejected</param>
    void OnPaymentModeRejected(string reason);
}