using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using TradingSystem.inventory.data;
using TradingSystem.inventory.data.enterprise;
using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.application.store;

public class StoreApplication : IStoreApplication, ICashDeskConnector
{
    private readonly IStoreQuery _storeQuery = IDataFactory.GetInstance().GetStoreQuery();
    private readonly long _storeId;

    public StoreApplication(long storeId)
    {
        _storeId = storeId;
    }

    public Store GetStore()
    {
        Store result = new();

        using var dbc = new DatabaseContext();
        using var ctx = dbc.Database.BeginTransaction();
        
        try
        {
            result = _storeQuery.QueryStoreById(_storeId, dbc);
            ctx.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return result;
    }

    public List<StockItem> GetProductsLowStockItems()
    {
        List<StockItem> result = new();
        using var dbc = new DatabaseContext();
        using var ctx = dbc.Database.BeginTransaction();

        try
        {
            result = _storeQuery.QueryLowStockItems(_storeId, dbc);
            ctx.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return result;
    }

    public List<Product> GetAllProductSuppliers()
    {
        List<Product> result = new();
        using var dbc = new DatabaseContext();
        using var ctx = dbc.Database.BeginTransaction();
        
        try
        {
            result = _storeQuery.QueryProducts(_storeId, dbc);
            ctx.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return result;
    }

    public List<StockItem> GetAllProductsSupplierStockItems()
    {
        List<StockItem> result = new();
        using var dbc = new DatabaseContext();
        using var ctx = dbc.Database.BeginTransaction();

        try
        {
            result = _storeQuery.QueryAllProductStockItems(_storeId, dbc);
            ctx.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        return result;
    }

    public void OrderProducts(ProductOrder productOrder)
    {
        using var dbc = new DatabaseContext();
        using var trans = dbc.Database.BeginTransaction();
        try
        {
            dbc.ProductOrders.Attach(productOrder);
            dbc.SaveChanges();
            trans.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public ProductOrder GetProductOrder(long productOrderId)
    {
        ProductOrder result = new();
        using var dbc = new DatabaseContext();
        using var ctx = dbc.Database.BeginTransaction();

        try
        {
            result = _storeQuery.QueryOrderById(productOrderId, dbc);
            ctx.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        return result;
    }

    public void RollInReceivedProductOrder(long productOrderId)
    {
        using var dbc = new DatabaseContext();
        using var trans = dbc.Database.BeginTransaction();
        try
        {
            var result = _storeQuery.QueryOrderById(productOrderId, dbc);
            result.DeliveryDate = DateTime.UtcNow;

            foreach (var oe in result.OrderEntries)
            {
                var item = _storeQuery.QueryStockItem(_storeId, oe.Product.Barcode, dbc);
                item.Amount += oe.Amount;
            }
            
            dbc.SaveChanges();
            trans.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void ChangePrice(StockItem stockItem)
    {
        using var dbc = new DatabaseContext();
        using var ctx = dbc.Database.BeginTransaction();
        try
        {
            var result = _storeQuery.QueryStockItemById(stockItem.Id, dbc);
            result.SalesPrice = stockItem.SalesPrice;
            dbc.SaveChanges();
            ctx.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void BookSale(Sale sale)
    {
        throw new NotImplementedException();
    }

    public StockItem GetProductStockItem(long productBarcode)
    {
        StockItem result = new();
        using var dbc = new DatabaseContext();
        using var ctx = dbc.Database.BeginTransaction();

        try
        {
            result = _storeQuery.QueryStockItem(_storeId, productBarcode, dbc);
            ctx.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        return result;
    }
}