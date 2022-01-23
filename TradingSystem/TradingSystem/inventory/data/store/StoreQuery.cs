using Microsoft.EntityFrameworkCore;
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
        var result = _database
                         .Stores
                         .Find(storeId) 
                     ?? throw new ArgumentException($"Can't find store by id {storeId}");
        return result;
    }

    public List<Product> QueryProducts(long storeId)
    {
        var result = _database
                         .StockItems
                         .Where(st => st.Store.Id == storeId)
                         .Select(st => st.Product)
                         .ToList() 
                     ?? throw new Exception();
        return result;
    }

    public Product QueryProductById(long productId)
    {
        var result = _database
                         .Products
                         .Find(productId)
                     ?? throw new ArgumentException($"Can't find product by id {productId}");
        return result;
    }

    public List<StockItem> QueryLowStockItems(long storeId)
    {
        var result = _database
                         .StockItems
                         .Where(st => (st.Store.Id == storeId) && (st.Amount < st.MinStock))
                         .ToList()
                     ?? throw new Exception();
        return result;
    }

    public List<StockItem> QueryAllStockItems(long storeId)
    {
        var result = _database
                         .StockItems
                         .Where(st => st.Store.Id == storeId)
                         .ToList()
                     ?? throw new Exception();
        return result;
    }

    public ProductOrder QueryOrderById(long orderId)
    {
        var result = _database.ProductOrders
                         .Find(orderId)
                     ?? throw new ArgumentException($"Can't find order by id {orderId}");
        return result;
    }

    public StockItem QueryStockItem(long storeId, long barcode)
    {
        var result = _database
                         .StockItems
                         .Include(st => st.Product)
                         .Single(st => (st.Store.Id == storeId) && (st.Product.Barcode == barcode))
                     ?? throw new Exception();
        return result;
    }

    public List<StockItem> QueryStockItems(long storeId, long[] productIds)
    {
        var result = _database
                        .StockItems
                        .Include(st => st.Product)
                        .Where(st => (st.Store.Id == storeId) && (productIds.Contains(st.Product.Id)))
                        .ToList() 
                     ?? throw new Exception();
        return result;
    }

    public StockItem QueryStockItemById(long stockId)
    {
        var result = _database
                         .StockItems
                         .Include(st => st.Product)
                         .Single(st => st.Id == stockId)
                     ?? throw new Exception();
        return result;
    }
}