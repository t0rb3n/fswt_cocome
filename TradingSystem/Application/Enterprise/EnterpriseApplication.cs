using Application.Exceptions;
using Application.Mappers;
using Application.Store;
using Data;
using Data.Enterprise;
using Data.Exceptions;
using Data.Store;

namespace Application.Enterprise;

public class EnterpriseApplication : IEnterpriseApplication
{
    private readonly IStoreQuery _storeQuery = IDataFactory.GetInstance().GetStoreQuery();
    private readonly IEnterpriseQuery _enterpriseQuery = IDataFactory.GetInstance().GetEnterpriseQuery();
    private readonly long _enterpriseId;

    public EnterpriseApplication(long enterpriseId)
    {
        _enterpriseId = enterpriseId;
    }

    public EnterpriseDTO GetEnterprise()
    {
        EnterpriseDTO result;
        using var dbc = new DatabaseContext();
        using var transaction = dbc.Database.BeginTransaction();
        
        try
        {
            var query = _enterpriseQuery.QueryEnterpriseById(_enterpriseId, dbc);
            result = EntryObject.ToEnterpriseDTO(query);
            transaction.Commit();
        }
        catch (ItemNotFoundException e)
        {
            Console.WriteLine(e);
            throw new EnterpriseException("Enterprise could not be found!");
        }

        return result;
    }

    public IList<StoreDTO> GetEnterpriseStores()
    {
        List<StoreDTO> result = new();
        using var dbc = new DatabaseContext();
        using var transaction = dbc.Database.BeginTransaction();
        
        try
        {
            var query = _enterpriseQuery.QueryStores(_enterpriseId, dbc);
            result.AddRange(query.Select(EntryObject.ToStoreDTO));
            transaction.Commit();
        }
        catch (ItemNotFoundException e)
        {
            Console.WriteLine(e);
            throw new EnterpriseException(
                "An unexpected error occurred while receiving the store list!");
        }

        return result;
    }
    
    public IList<ProductSupplierDTO> GetEnterpriseProductSupplier()
    {
        List<ProductSupplierDTO> result = new();
        using var dbc = new DatabaseContext();
        using var transaction = dbc.Database.BeginTransaction();
        
        try
        {
            var query = _enterpriseQuery.QueryProductSuppliers(_enterpriseId, dbc);
            result.AddRange(query.Select(EntryObject.ToProductSupplierDTO));
            transaction.Commit();
        }
        catch (ItemNotFoundException e)
        {
            Console.WriteLine(e);
            throw new EnterpriseException(
                "An unexpected error occurred while receiving the product supplier list!");
        }

        return result;
    }

    public ReportDTO GetStockReport(Data.Store.Store store)
    {
        throw new NotImplementedException();
    }

    public ReportDTO GetStockReport(Data.Enterprise.Enterprise enterprise)
    {
        throw new NotImplementedException();
    }

    public ReportDTO GetMeanTimeToDeliveryReport(Data.Enterprise.Enterprise enterprise)
    {
        ReportDTO result = new();
        
        using var dbc = new DatabaseContext();
        using var transaction = dbc.Database.BeginTransaction();
        
        try
        {
            var query = _enterpriseQuery.QueryProductSuppliers(_enterpriseId, dbc);
            foreach (var supplier in query)
            {
               var mean = _enterpriseQuery.QueryMeanTimeToDelivery(supplier, enterprise, dbc);
               result.ReportText += $"Supplier: {supplier.Name} = {TimeSpan.FromTicks(mean)} ms\n";
            }

            transaction.Commit();
        }
        catch (ItemNotFoundException e)
        {
            Console.WriteLine(e);
            throw new EnterpriseException(
                "An unexpected error occurred while calculating the average delivery time!");
        }

        return result;
    }

    public StoreEnterpriseDTO GetStore(long storeId)
    {
        StoreEnterpriseDTO result;
        using var dbc = new DatabaseContext();
        using var transaction = dbc.Database.BeginTransaction();

        try
        {
            var query = _storeQuery.QueryStoreById(storeId, dbc);
            result = EntryObject.ToStoreEnterpriseDTO(query);
            transaction.Commit();
        }
        catch (ItemNotFoundException e)
        {
            Console.WriteLine(e);
            throw new EnterpriseException("Store could not be found!");
        }

        return result;
    }

    public IList<ProductStockItemDTO> GetProductsLowStockItems(long storeId)
    {
        List<ProductStockItemDTO> result = new();
        using var dbc = new DatabaseContext();
        using var transaction = dbc.Database.BeginTransaction();

        try
        {
            var query = _storeQuery.QueryLowStockItems(storeId, dbc);
            result.AddRange(query.Select(EntryObject.ToProductStockItemDTO));
            transaction.Commit();
        }
        catch (ItemNotFoundException e)
        {
            Console.WriteLine(e);
            throw new EnterpriseException(
                "An unexpected error occurred while receiving the product stock item list!");
        }

        return result;
    }

    public IList<ProductSupplierDTO> GetAllProductSuppliers(long storeId)
    {
        List<ProductSupplierDTO> result = new();
        using var dbc = new DatabaseContext();
        using var transaction = dbc.Database.BeginTransaction();
        
        try
        {
            var query = _storeQuery.QueryProducts(storeId, dbc);
            result.AddRange(query.Select(EntryObject.ToProductSupplierDTO));
            transaction.Commit();
        }
        catch (ItemNotFoundException e)
        {
            Console.WriteLine(e);
            throw new EnterpriseException(
                "An unexpected error occurred while receiving the product supplier list!");
        }

        return result;
    }

    public IList<ProductSupplierStockItemDTO> GetAllProductSupplierStockItems(long storeId)
    {
        List<ProductSupplierStockItemDTO> result = new();
        using var dbc = new DatabaseContext();
        using var transaction = dbc.Database.BeginTransaction();

        try
        {
            var query = _storeQuery.QueryAllProductStockItems(storeId, dbc);
            result.AddRange(query.Select(EntryObject.ToProductSupplierStockItemDTO));
            transaction.Commit();
        }
        catch (ItemNotFoundException e)
        {
            Console.WriteLine(e);
            throw new EnterpriseException(
                "An unexpected error occurred while receiving the product supplier stock item list!");
        }
        
        return result;
    }

    public void OrderProducts(ProductOrderDTO productOrder, long storeId)
    {
        using var dbc = new DatabaseContext();
        using var transaction = dbc.Database.BeginTransaction();
        
        try
        {
            ProductOrder poe = new();
            foreach (var order in productOrder.Orders)
            {
                var product = _storeQuery.QueryProductById(order.ProductSupplier.ProductId, dbc);
                var oe = new OrderEntry
                {
                    Amount = order.Amount,
                    Product = product
                };
                poe.OrderEntries.Add(oe);
            }

            poe.Store = _storeQuery.QueryStoreById(storeId, dbc);
            poe.OrderingDate = productOrder.OrderingDate;
            dbc.ProductOrders.Attach(poe);
            dbc.SaveChanges();
            transaction.Commit();
        }
        catch (ItemNotFoundException e)
        {
            Console.WriteLine(e);
            throw new EnterpriseException(
                "An unexpected error occurred while executing the order!");
        }
    }

    public ProductOrderDTO GetProductOrder(long productOrderId)
    {
        ProductOrderDTO result;
        using var dbc = new DatabaseContext();
        using var transaction = dbc.Database.BeginTransaction();

        try
        {
            var query = _storeQuery.QueryOrderById(productOrderId, dbc);
            result = EntryObject.ToProductOrderDTO(query);
            transaction.Commit();
        }
        catch (ItemNotFoundException e)
        {
            Console.WriteLine(e);
            throw new EnterpriseException("Product order could not be found!");
        }
        
        return result;
    }

    public void RollInReceivedProductOrder(ProductOrderDTO productOrder, long storeId)
    {
        using var dbc = new DatabaseContext();
        using var transaction = dbc.Database.BeginTransaction();
        try
        {
            var result = _storeQuery.QueryOrderById(productOrder.ProductOrderId, dbc);

            if (result.DeliveryDate != DateTime.MinValue)
            {
                throw new EnterpriseException("Product order has already been received!");
            }

            if (result.Store.Id != storeId)
            {
                throw new EnterpriseException("Product order can not be executed from this store!");
            }
            
            result.DeliveryDate = productOrder.DeliveryDate;

            foreach (var oe in result.OrderEntries)
            {
                var item = _storeQuery.QueryStockItem(storeId, oe.Product.Barcode, dbc);
                item.Amount += oe.Amount;
            }
            
            dbc.SaveChanges();
            transaction.Commit();
        }
        catch (ItemNotFoundException e)
        {
            Console.WriteLine(e);
            throw new EnterpriseException("Product order could not be found!");
        }
    }

    public void ChangePrice(long stockItemId, double newPrice)
    {
        using var dbc = new DatabaseContext();
        using var transaction = dbc.Database.BeginTransaction();
        try
        {
            var result = _storeQuery.QueryStockItemById(stockItemId, dbc);
            result.SalesPrice = newPrice;
            dbc.SaveChanges();
            transaction.Commit();
        }
        catch (ItemNotFoundException e)
        {
            Console.WriteLine(e);
            throw new EnterpriseException("An unexpected error occurred while changing the price!");
        }
    }
    
    public void MakeBookSale(SaleDTO saleDto)
    {
        using var dbc = new DatabaseContext();
        using var transaction = dbc.Database.BeginTransaction();

        try
        {
            foreach (var product in saleDto.Products)
            {
                var query = _storeQuery.QueryStockItemById(product.StockItem.ItemId, dbc);
                //TODO: what should happen if a stock item is no longer available?
                if (query.Amount > 0) query.Amount -= 1;
                dbc.SaveChanges();
            }

            transaction.Commit();
        }
        catch (ItemNotFoundException e)
        {
            Console.WriteLine(e);
            throw new EnterpriseException("An unexpected error occurred while booking the stock items!");
        }
    }

    public ProductStockItemDTO GetProductStockItem(long productBarcode, long storeId)
    {
        ProductStockItemDTO result;
        using var dbc = new DatabaseContext();
        using var transaction = dbc.Database.BeginTransaction();

        try
        {
            var query = _storeQuery.QueryStockItem(storeId, productBarcode, dbc);
            result = EntryObject.ToProductStockItemDTO(query);
            transaction.Commit();
        }
        catch (ItemNotFoundException e)
        {
            Console.WriteLine(e);
            throw new EnterpriseException("Product stock item could not be found!");
        }
        
        return result;
    }
}