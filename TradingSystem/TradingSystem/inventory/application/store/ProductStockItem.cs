using TradingSystem.inventory.data.enterprise;
using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.application.store;

public class ProductStockItem : Product
{
    protected StockItem StockItem { get; set; }
}