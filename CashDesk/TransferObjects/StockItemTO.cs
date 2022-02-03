namespace CashDesk.TransferObjects;

public class StockItemTO
{
    private long Id { get; set; }
    private double SalesPrice{ get; set; }
    private long Amount { get; set; }
    private long MinStock { get; set; }
    private long MaxStock { get; set; }
    
}