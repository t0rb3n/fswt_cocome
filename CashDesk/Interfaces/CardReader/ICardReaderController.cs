namespace CashDesk.CardReader;

public interface ICardReaderController
{
    void SendCreditCardInfo(string cardInfo);
    void SendCreditCardPin(int pin);
}