namespace TradingSystem.inventory.application.store;

public class Sale
{
    protected DateOnly date { get; set; }
    protected List<ProductStockItem> products { get; set; }
}