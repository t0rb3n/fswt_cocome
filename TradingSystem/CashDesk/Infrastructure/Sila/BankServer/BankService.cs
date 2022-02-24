using CashDesk.Application.Interfaces;
using CashDesk.CardReaderService;
using Tecan.Sila2;

namespace CashDesk.Infrastructure.Sila.BankServer;

public class BankService : IBankService
{
    public event EventHandler<string>? PaymentModeRejected;

    private readonly CardReaderServiceClient _cardReaderClient;
    private readonly IBankServer _bankClient;
    private readonly ILogger<BankService> _logger;


    public BankService(
        ILogger<BankService> logger,
        IBankServer bankServerClient,
        CardReaderServiceClient cardReaderClient)
    {
        _logger = logger;
        _bankClient = bankServerClient;
        _cardReaderClient = cardReaderClient;
    }


    public async Task TryPayingByCard(long amount)
    {
        var context = _bankClient.CreateContext(amount);
        var authorizeCommand = _cardReaderClient.Authorize(amount, context.Challenge);
        var token = await authorizeCommand.Response;
        try
        {
            _bankClient.AuthorizePayment(context.ContextId, token.Account, token.AuthorizationToken);
            _cardReaderClient.Confirm();
        }
        catch (SilaException ex)
        {
            _cardReaderClient.Abort(ex.Message);

            if (ex.Message.StartsWith("InsufficientCreditException"))
            {
                const string reason = "Card has insufficient balance.";
                OnPaymentModeRejected(reason);
            }
            else
            {
                const string reason = "Error with the credit card.";
                OnPaymentModeRejected(reason);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Something went wrong when talking to the bank, reason: {Reason} ", ex.Message);
            throw;
        }
    }

    public void OnPaymentModeRejected(string reason)
    {
        PaymentModeRejected?.Invoke(this, reason);
    }
}