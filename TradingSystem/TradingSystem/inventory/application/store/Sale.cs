namespace TradingSystem.inventory.application.store;

public class Sale
{
    protected DateTime Date { get; set; }
    protected List<ProductStockItem> Products { get; set; }
}