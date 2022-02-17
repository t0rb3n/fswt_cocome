using CashDesk.Classes.EventArgs;

namespace CashDesk.Classes;

public class Sale : SaleRegisteredArgs
{
    public DateTime Date { get; init; }
}