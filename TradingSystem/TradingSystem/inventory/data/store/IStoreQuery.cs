using TradingSystem.inventory.data.enterprise;

namespace TradingSystem.inventory.data.store;

public interface IStoreQuery
{
    public Store QueryStoreById(long storeId, DatabaseContext dbc);
    public List<Product> QueryProducts(long storeId, DatabaseContext dbc);
    public Product QueryProductById(long productId, DatabaseContext dbc);
    public List<StockItem> QueryLowStockItems(long storeId, DatabaseContext dbc);
    public List<StockItem> QueryAllStockItems(long storeId, DatabaseContext dbc);
    public List<StockItem> QueryAllProductStockItems(long storeId, DatabaseContext dbc);
    public ProductOrder QueryOrderById(long orderId, DatabaseContext dbc);
    public StockItem QueryStockItem(long storeId, long barcode, DatabaseContext dbc);
    public List<StockItem> QueryStockItems(long storeId, long[] productIds, DatabaseContext dbc);
    public StockItem QueryStockItemById(long stockId, DatabaseContext dbc); 
}