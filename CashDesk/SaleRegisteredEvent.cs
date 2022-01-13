namespace CashDesk
{
    public sealed class SaleRegisteredEvent
    {
        private readonly int cashDeskId;

        private readonly int itemCount;

        private readonly PaymentMethod paymentMethod;

        public SaleRegisteredEvent(int cashDeskid, int itemCount, PaymentMethod paymentMethod)
        {
            this.cashDeskId = cashDeskid;
            this.itemCount = itemCount;
            this.paymentMethod = paymentMethod;
        }

        public int CashDeskId { get;}

        public int ItemCount { get;}

        public PaymentMethod PaymentMethod { get;}

    }

}