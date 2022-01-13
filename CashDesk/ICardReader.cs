namespace CashDesk
{
    public interface ICardReader
    {
        void sendCreditCardInfo(string creditCardInfo);

        void sendCreditCardPin(int pin);
    }
}