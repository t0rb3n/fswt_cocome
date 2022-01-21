namespace CashDesk.Bank;

public interface IBank
{
    TransactionID ValidateCard(string cardInfo, int pinNumber);
    DebitResult DebitCard(TransactionID id);
}