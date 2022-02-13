using Data.Enterprise;

namespace Data.Store;

public interface IStoreQuery
{
    public Store QueryStoreById(long storeId, DatabaseContext dbc);
    public IList<Product> QueryProducts(long storeId, DatabaseContext dbc);
    public Product QueryProductById(long productId, DatabaseContext dbc);
    public IList<StockItem> QueryLowStockItems(long storeId, DatabaseContext dbc);
    public IList<StockItem> QueryAllStockItems(long storeId, DatabaseContext dbc);
    public IList<StockItem> QueryAllProductStockItems(long storeId, DatabaseContext dbc);
    public ProductOrder QueryOrderById(long orderId, DatabaseContext dbc);
    public StockItem QueryStockItem(long storeId, long barcode, DatabaseContext dbc);
    public IList<StockItem> QueryStockItems(long storeId, long[] productIds, DatabaseContext dbc);
    public StockItem QueryStockItemById(long stockId, DatabaseContext dbc); 
}