using TradingSystem.inventory.data.enterprise;

namespace TradingSystem.inventory.data.store;

public class StoreQuery : IStoreQuery
{
    private readonly DataContext _database;

    public StoreQuery(DataContext db)
    {
        _database = db;
    }
    public Store QueryStoreById(long storeId)
    {
        throw new NotImplementedException();
    }

    public List<Product> QueryProducts(long storeId)
    {
        throw new NotImplementedException();
    }

    public Product QueryProductById(long productId)
    {
        throw new NotImplementedException();
    }

    public List<StockItem> QueryLowStockItems(long storeId)
    {
        throw new NotImplementedException();
    }

    public List<StockItem> QueryAllStockItems(long storeId)
    {
        throw new NotImplementedException();
    }

    public ProductOrder QueryOrderById(long orderId)
    {
        throw new NotImplementedException();
    }

    public StockItem QueryStockItem(long storeId, long barcode)
    {
        throw new NotImplementedException();
    }

    public List<StockItem> QueryStockItems(long storeId, long[] productIds)
    {
        throw new NotImplementedException();
    }

    public StockItem QueryStockItemById(long stockId)
    {
        throw new NotImplementedException();
    }
}