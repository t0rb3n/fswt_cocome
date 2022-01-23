using TradingSystem.inventory.data.enterprise;

namespace TradingSystem.inventory.data.store;

public interface IStoreQuery
{
    public Store QueryStoreById(long storeId);
    public List<Product> QueryProducts(long storeId);
    public Product QueryProductById(long productId);
    public List<StockItem> QueryLowStockItems(long storeId);
    public List<StockItem> QueryAllStockItems(long storeId);
    public ProductOrder QueryOrderById(long orderId);
    public StockItem QueryStockItem(long storeId, long barcode);
    public List<StockItem> QueryStockItems(long storeId, long[] productIds);
    public StockItem QueryStockItemById(long stockId);
}