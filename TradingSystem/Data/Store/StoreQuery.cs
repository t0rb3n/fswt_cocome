using Microsoft.EntityFrameworkCore;
using Data.Enterprise;
using Data.Exceptions;

namespace Data.Store;

/// <summary>
/// Class <c>StoreQuery</c> implemented the interfaces of IStoreQuery.
/// </summary>
public class StoreQuery : IStoreQuery
{
    public Store QueryStoreById(long storeId, DatabaseContext dbc)
    {
        Store result;
        try
        {
            result = dbc
                .Stores
                .Find(storeId)!;
            dbc.Entry(result).Reference(store => store.Enterprise).Load();
        }
        catch (ArgumentNullException)
        {
            throw new ItemNotFoundException($"Store with the id '{storeId}' could not be found!");
        }
        return result;
    }

    public IList<Product> QueryProductSuppliers(long storeId, DatabaseContext dbc)
    {
        List<Product> result;
        try
        {
            result = dbc
                .StockItems
                .Where(item => item.Store.Id == storeId)
                .Include(item => item.Product.ProductSupplier)
                .Select(item => item.Product)
                .ToList();

            if (result.Count == 0)
            {
                throw new ItemNotFoundException($"Products from store id '{storeId}' could not be found!");
            }
        }
        catch (ArgumentNullException)
        {
            throw new ItemNotFoundException($"Products from store id '{storeId}' could not be found!");
        }
        return result;
    }

    public Product QueryProductById(long productId, DatabaseContext dbc)
    {
        Product result;
        try
        {
            result = dbc
                .Products
                .Find(productId)!;

            if (result == null)
            {
                throw new ItemNotFoundException($"Product with id '{productId}' could not be found!");
            }
        }
        catch (ArgumentNullException)
        {
            throw new ItemNotFoundException($"Product with id '{productId}' could not be found!");
        }
        return result;
    }

    public IList<StockItem> QueryLowProductSupplierStockItems(long storeId, DatabaseContext dbc)
    {
        List<StockItem> result;
        try
        {
            result = dbc
                .StockItems
                .Where(item => item.Store.Id == storeId && item.Amount < item.MinStock)
                .Include(item => item.Product)
                .ThenInclude(product => product.ProductSupplier)
                .ToList();

            if (result.Count == 0)
            {
                throw new ItemNotFoundException($"Stock items from store id '{storeId}' could not be found!");
            }
        }
        catch (ArgumentNullException)
        {
            throw new ItemNotFoundException($"Stock items from store id '{storeId}' could not be found!");
        }
        return result;
    }

    public IList<StockItem> QueryAllProductStockItems(long storeId, DatabaseContext dbc)
    {
        List<StockItem> result;
        try
        {
            result = dbc
                       .StockItems
                       .Where(item => item.Store.Id == storeId)
                       .Include(item => item.Product)
                       .ToList();
            
            if (result.Count == 0)
            {
                throw new ItemNotFoundException($"Stock items from store id '{storeId}' could not be found!");
            }
        }
        catch (ArgumentNullException)
        {
            throw new ItemNotFoundException($"Stock items from store id '{storeId}' could not be found!");
        }
        return result;
    }
    
    public IList<StockItem> QueryAllProductSupplierStockItems(long storeId, DatabaseContext dbc)
    {
        List<StockItem> result;
        try
        {
            result = dbc
                .StockItems
                .Where(item => item.Store.Id == storeId)
                .Include(item => item.Product)
                .ThenInclude(product => product.ProductSupplier)
                .ToList();
            
            if (result.Count == 0)
            {
                throw new ItemNotFoundException($"Product stock items from store id '{storeId}' could not be found!");
            }
        }
        catch (ArgumentNullException)
        {
            throw new ItemNotFoundException($"Product stock items from store id '{storeId}' could not be found!");
        }
        return result;
    }

    public ProductOrder QueryProductOrderById(long orderId, DatabaseContext dbc)
    {
        ProductOrder result;
        try
        {
            result = dbc.ProductOrders
                .Include(order => order.OrderEntries)
                .ThenInclude(entry => entry.Product)
                .ThenInclude(product => product.ProductSupplier)
                .Single(order => order.Id == orderId);
        }
        catch (ArgumentNullException)
        {
            throw new ItemNotFoundException($"Product order with id '{orderId}' could not be found!");
        }
        catch (InvalidOperationException)
        {
            throw new ItemNotFoundException($"Product order with id '{orderId}' could not be found!");
        }
        
        return result;
    }
    
    public IList<ProductOrder> QueryAllProductOrders(long storeId, DatabaseContext dbc)
    {
        List<ProductOrder> result;
        try
        {
            result = dbc.ProductOrders
                .Where(order => order.Store.Id == storeId)
                .Include(order => order.OrderEntries)
                .ThenInclude(entry => entry.Product)
                .ThenInclude(product => product.ProductSupplier)
                .ToList();
            
            if (result.Count == 0)
            {
                throw new ItemNotFoundException($"Product orders from store id '{storeId}' could not be found!");
            }
        }
        catch (ArgumentNullException)
        {
            throw new ItemNotFoundException($"Product orders from store id '{storeId}' could not be found!");
        }
        return result;
    }
    
    public IList<ProductOrder> QueryAllOpenProductOrders(long storeId, DatabaseContext dbc)
    {
        List<ProductOrder> result;
        try
        {
            result = dbc.ProductOrders
                .Where(order => order.Store.Id == storeId && order.DeliveryDate == DateTime.MinValue)
                .Include(order => order.OrderEntries)
                .ThenInclude(entry => entry.Product)
                .ThenInclude(product => product.ProductSupplier)
                .ToList();
            
            if (result.Count == 0)
            {
                throw new ItemNotFoundException($"Open product orders from store id '{storeId}' could not be found!");
            }
        }
        catch (ArgumentNullException)
        {
            throw new ItemNotFoundException($"Open product orders from store id '{storeId}' could not be found!");
        }
        return result;
    }

    public StockItem QueryProductStockItem(long storeId, long barcode, DatabaseContext dbc)
    {
        StockItem result;
        try
        {
            result = dbc
                .StockItems
                .Single(item => item.Store.Id == storeId && item.Product.Barcode == barcode);
            dbc.Entry(result).Reference(item => item.Product).Load();
        }
        catch (ArgumentNullException)
        {
            throw new ItemNotFoundException($"Stock item with barcode '{barcode}' could not be found!");
        }
        catch (InvalidOperationException)
        {
            throw new ItemNotFoundException($"Stock item with barcode '{barcode}' could not be found!");
        }
        
        return result;
    }

    public IList<StockItem> QueryProductStockItems(long storeId, long[] productIds, DatabaseContext dbc)
    {
        List<StockItem> result;
        try
        {
            if (productIds.Length == 0)
            {
                throw new ArgumentNullException();
            }
            
            result = dbc
                .StockItems
                .Where(item => item.Store.Id == storeId && productIds.Contains(item.Product.Id))
                .Include(item => item.Product)
                .ToList();

            if (result.Count != productIds.Length)
            {
                throw new ItemNotFoundException("One or more stock items in the product list could not be found!");
            }
        }
        catch (ArgumentNullException)
        {
            throw new ItemNotFoundException("Stock items could not be found because the productIds array was empty!");
        }
        
        return result;
    }

    public StockItem QueryProductStockItemById(long stockId, DatabaseContext dbc)
    {
        StockItem result;
        try
        {
            result = dbc
                .StockItems
                .Single(item => item.Id == stockId);
            dbc.Entry(result).Reference(item => item.Product).Load();
        }
        catch (ArgumentNullException)
        {
            throw new ItemNotFoundException($"Stock item with id '{stockId}' could not be found!");
        }
        catch (InvalidOperationException)
        {
            throw new ItemNotFoundException($"Stock item with id '{stockId}' could not be found!");
        }
        
        return result;
    }
}