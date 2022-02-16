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

    /// <summary>
    /// This constructor initializes a new enterprise application.
    /// </summary>
    /// <param name="enterpriseId">The id of your enterprise.</param>
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
            // Makes the query to the database.
            var query = _enterpriseQuery.QueryEnterpriseById(_enterpriseId, dbc);
            // Converts Entity object to DTO object.
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
            // Makes the query to the database.
            var query = _enterpriseQuery.QueryStores(_enterpriseId, dbc);
            // Converts Entity object to DTO object and adds to the result list.
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
            // Makes the query to the database.
            var query = _enterpriseQuery.QueryProductSuppliers(_enterpriseId, dbc);
            // Converts Entity object to DTO object and adds to the result list.
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
            // Makes the query to the database.
            var query = _enterpriseQuery.QueryProductSuppliers(_enterpriseId, dbc);
            // Determines for each supplier the  average time for its deliveries.
            foreach (var supplier in query)
            { 
                // Makes the query to the database.
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
            // Makes the query to the database.
            var query = _storeQuery.QueryStoreById(storeId, dbc);
            // Converts Entity object to DTO object.
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
            // Makes the query to the database.
            var query = _storeQuery.QueryLowStockItems(storeId, dbc);
            // Converts Entity object to DTO object and adds to the result list.
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
            // Makes the query to the database.
            var query = _storeQuery.QueryProducts(storeId, dbc);
            // Converts Entity object to DTO object and adds to the result list.
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
            // Makes the query to the database.
            var query = _storeQuery.QueryAllProductStockItems(storeId, dbc);
            // Converts Entity object to DTO object and adds to the result list.
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
            var poe = new ProductOrder
            {
                Id = 0
            };
            // Adds each order in the product order Entity object.
            foreach (var order in productOrder.Orders)
            {
                // Makes the query to the database.
                var product = _storeQuery.QueryProductById(order.ProductSupplier.ProductId, dbc);
                var oe = new OrderEntry
                {
                    Id = 0,
                    Amount = order.Amount,
                    Product = product
                };
                poe.OrderEntries.Add(oe);
            }

            // Makes the query to the database.
            poe.Store = _storeQuery.QueryStoreById(storeId, dbc);
            poe.OrderingDate = productOrder.OrderingDate;
            // Adds product order to the database.
            dbc.ProductOrders.Attach(poe);
            // Commits product order to the database.
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
            // Makes the query to the database.
            var query = _storeQuery.QueryOrderById(productOrderId, dbc);
            // Converts Entity object to DTO object.
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
            // Makes the query to the database.
            var result = _storeQuery.QueryOrderById(productOrder.ProductOrderId, dbc);

            // Checks if the product order has not been processed yet.
            if (result.DeliveryDate != DateTime.MinValue)
            {
                throw new EnterpriseException("Product order has already been received!");
            }

            // Checks if the product order belongs to this store.
            if (result.Store.Id != storeId)
            {
                throw new EnterpriseException("Product order can not be executed from this store!");
            }
            
            // Sets the delivery date
            result.DeliveryDate = productOrder.DeliveryDate;

            // Adds the received stock to the inventory of the specified store.
            foreach (var oe in result.OrderEntries)
            {
                // Makes the query to the database.
                var item = _storeQuery.QueryStockItem(storeId, oe.Product.Barcode, dbc);
                item.Amount += oe.Amount;
            }
            
            // Commits the changes of the Entities to the database.
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
            // Makes the query to the database.
            var result = _storeQuery.QueryStockItemById(stockItemId, dbc);
            // Changes the sale price of the stock item.
            result.SalesPrice = newPrice;
            // Commits the new sale price to the database.
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
                // Makes the query to the database.
                var query = _storeQuery.QueryStockItemById(product.StockItem.ItemId, dbc);
                // TODO: What should happen if a stock item is no longer available?
                // Withdraws the sold stock items in the store's warehouse as long as the stock is not at 0.
                if (query.Amount > 0) query.Amount -= 1;
                // Commits the stock to the database.
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
            // Makes the query to the database.
            var query = _storeQuery.QueryStockItem(storeId, productBarcode, dbc);
            // Converts Entity object to DTO object.
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
