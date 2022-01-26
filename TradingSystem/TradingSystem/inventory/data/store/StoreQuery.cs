using Microsoft.EntityFrameworkCore;
using TradingSystem.inventory.data.enterprise;

namespace TradingSystem.inventory.data.store;

public class StoreQuery : IStoreQuery
{
    public Store QueryStoreById(long storeId, DatabaseContext dbc)
    {
        var result = dbc
                         .Stores
                         .Find(storeId) 
                     ?? throw new Exception($"Can't find store by id {storeId}");
        dbc.Entry(result).Reference(store => store.Enterprise).Load();
        return result;
    }

    public List<Product> QueryProducts(long storeId, DatabaseContext dbc)
    {
        var result = dbc
                         .StockItems
                         .Where(item => item.Store.Id == storeId)
                         .Include(item => item.Product.ProductSupplier)
                         .Select(item => item.Product)
                         .ToList()
                     ?? throw new Exception("QueryProducts failed");
        return result;
    }

    public Product QueryProductById(long productId, DatabaseContext dbc)
    {
        var result = dbc
                         .Products
                         .Find(productId)
                     ?? throw new ArgumentException($"Can't find product by id {productId}");
        return result;
    }

    public List<StockItem> QueryLowStockItems(long storeId, DatabaseContext dbc)
    {
        var result = dbc
                         .StockItems
                         .Where(item => item.Store.Id == storeId && item.Amount < item.MinStock)
                         .Include(item => item.Product)
                         .ToList()
                     ?? throw new Exception("QueryLowStockItems failed");
        return result;
    }

    public List<StockItem> QueryAllStockItems(long storeId, DatabaseContext dbc)
    {
        var result = dbc
                         .StockItems
                         .Where(item => item.Store.Id == storeId)
                         .ToList()
                     ?? throw new Exception("QueryAllStockItems failed");
        return result;
    }
    
    public List<StockItem> QueryAllProductStockItems(long storeId, DatabaseContext dbc)
    {
        var result = dbc
                         .StockItems
                         .Where(item => item.Store.Id == storeId)
                         .Include(item => item.Product)
                         .ThenInclude(product => product.ProductSupplier)
                         .ToList()
                     ?? throw new Exception("QueryAllProductStockItems failed");
        return result;
    }

    public ProductOrder QueryOrderById(long orderId, DatabaseContext dbc)
    {
        var result = dbc.ProductOrders
                         .Include(order => order.OrderEntries)
                         .ThenInclude(entry => entry.Product)
                         .ThenInclude(product => product.ProductSupplier)
                         .Single(order => order.Id == orderId)
                     ?? throw new Exception($"Can't find order by id {orderId}");
        return result;
    }

    public StockItem QueryStockItem(long storeId, long barcode, DatabaseContext dbc)
    {
        var result = dbc
                         .StockItems
                         .Single(item => item.Store.Id == storeId && item.Product.Barcode == barcode)
                     ?? throw new Exception("QueryStockItem failed");
        dbc.Entry(result).Reference(item => item.Product).Load();
        return result;
    }

    public List<StockItem> QueryStockItems(long storeId, long[] productIds, DatabaseContext dbc)
    {
        var result = dbc
                         .StockItems
                         .Where(item => item.Store.Id == storeId && productIds.Contains(item.Product.Id))
                         .Include(item => item.Product)
                         .ToList()
                     ?? throw new Exception("QueryStockItems failed");
        return result;
    }

    public StockItem QueryStockItemById(long stockId, DatabaseContext dbc)
    {
        var result = dbc
                         .StockItems
                         .Single(item => item.Id == stockId)
                     ?? throw new Exception($"Can't stockItem by id {stockId}");
        dbc.Entry(result).Reference(item => item.Product).Load();
        return result;
    }
}