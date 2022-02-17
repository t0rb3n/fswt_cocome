using CashDesk.Classes.Enums;

namespace CashDesk.Classes.EventArgs;

public class SaleRegisteredArgs : System.EventArgs
{
    public int Amount { get; init; }
    public PaymentMode Mode { get; init; }
}