using TradingSystem.inventory.data.enterprise;

namespace TradingSystem.inventory.data.store;

public class StoreQuery : IStoreQuery
{
    public Store queryStoreById(long storeId)
    {
        throw new NotImplementedException();
    }

    public List<Product> queryProducts(long storeId)
    {
        throw new NotImplementedException();
    }

    public Product queryProductById(long productId)
    {
        throw new NotImplementedException();
    }

    public List<StockItem> queryLowStockItems(long storeId) 
    {
        throw new NotImplementedException();
    }

    public List<StockItem> queryAllStockItems(long storeId)
    {
        throw new NotImplementedException();
    }

    public ProductOrder queryOrderById(long orderId)
    {
        throw new NotImplementedException();
    }

    public StockItem queryStockItem(long storeId, long barcode)
    {
        throw new NotImplementedException();
    }

    public List<StockItem> queryStockItems(long storeId, long[] productIds)
    {
        throw new NotImplementedException();
    }

    public StockItem queryStockItemById(long stockId)
    {
        throw new NotImplementedException();
    }
}