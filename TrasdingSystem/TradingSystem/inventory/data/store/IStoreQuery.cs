using TradingSystem.inventory.data.enterprise;

namespace TradingSystem.inventory.data.store;

public interface IStoreQuery
{
    public Store queryStoreById(long storeId);
    public List<Product> queryProducts(long storeId);
    public Product queryProductById(long productId);
    public List<StockItem> queryLowStockItems(long storeId);
    public List<StockItem> queryAllStockItems(long storeId);
    public ProductOrder queryOrderById(long orderId);
    public StockItem queryStockItem(long storeId, long barcode);
    public List<StockItem> queryStockItems(long storeId, long[] productIds);
    public StockItem queryStockItemById(long stockId);
}