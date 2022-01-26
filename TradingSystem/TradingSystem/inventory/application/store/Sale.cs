using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.application.store;

public class Sale
{
    protected DateTime Date { get; set; }
    protected List<StockItem> Products { get; set; }
}