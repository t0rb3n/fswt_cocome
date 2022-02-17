namespace CashDesk.Classes.EventArgs;

public class ChangeRunningTotalArgs : System.EventArgs
{
    public string ProductName { get; init; }
    public double Price { get; init; }
    public double Total { get; init; }
}