using Application.Exceptions;
using Application.Mappers;
using Application.Store;
using Data;
using Data.Enterprise;
using Data.Exceptions;
using Data.Store;

namespace Application.Enterprise;

/// <summary>
/// Class <c>EnterpriseApplication</c> implemented the interfaces of IEnterpriseApplication and IReporting.
/// </summary>
public class EnterpriseApplication : IEnterpriseApplication, IReporting
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

    public StoreStockReportDTO GetStoreStockReport(long storeId)
    {
        StoreStockReportDTO result;

        using var dbc = new DatabaseContext();
        using var transaction = dbc.Database.BeginTransaction();

        try
        {
            // Makes the query to the database.
            var store = _storeQuery.QueryStoreById(storeId, dbc);
            var stockitems = _storeQuery.QueryAllProductSupplierStockItems(storeId, dbc);

            result = new StoreStockReportDTO
            {
                StoreId = store.Id,
                StoreName = store.Name,
                Location = store.Location
            };
            // Converts Entity object to DTO object and adds to the result stockItem list.
            result.StockItems.AddRange(stockitems.Select(EntryObject.ToProductStockItemDTO));
        }
        catch (ItemNotFoundException e)
        {
            Console.WriteLine(e);
            throw new EnterpriseException(
                "An unexpected error occurred while creating the store stock report!");
        }

        return result;
    }

    public EnterpriseStockReportDTO GetEnterpriseStockReport(long enterpriseId)
    {
        EnterpriseStockReportDTO result;

        using var dbc = new DatabaseContext();
        using var transaction = dbc.Database.BeginTransaction();

        try
        {
            // Makes the query to the database.
            var enterprise = _enterpriseQuery.QueryEnterpriseById(enterpriseId, dbc);
            var stores =  _enterpriseQuery.QueryStores(enterpriseId, dbc);

            result = new EnterpriseStockReportDTO
            {
                EnterpriseId = enterprise.Id,
                EnterpriseName = enterprise.Name
            };
            // Creates a report for each store and add it to the StoreReport list.
            foreach (var store in stores)
            {
                result.StoreReports.Add(GetStoreStockReport(store.Id));
            }
        }
        catch (ItemNotFoundException e)
        {
            Console.WriteLine(e);
            throw new EnterpriseException(
                "An unexpected error occurred while creating the enterprise stock report!");
        }

        return result;
    }

    public IList<SupplierMeanTimeReportDTO> GetMeanTimeToDeliveryReport(long enterpriseId)
    {
        var result = new List<SupplierMeanTimeReportDTO>();
        
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
               var mean = _enterpriseQuery.QueryMeanTimeToDelivery(supplier.Id, enterpriseId, dbc);
               result.Add(new SupplierMeanTimeReportDTO
               {
                   SupplierId = supplier.Id,
                   SupplierName = supplier.Name,
                   MeanTime = TimeSpan.FromTicks(mean)
               });
            }

            transaction.Commit();
        }
        catch (ItemNotFoundException e)
        {
            Console.WriteLine(e);
            throw new EnterpriseException(
                "An unexpected error occurred while calculating the mean delivery time of a supplier!");
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

    public IList<ProductSupplierStockItemDTO> GetProductsLowStockItems(long storeId)
    {
        List<ProductSupplierStockItemDTO> result = new();
        using var dbc = new DatabaseContext();
        using var transaction = dbc.Database.BeginTransaction();

        try
        {
            // Makes the query to the database.
            var query = _storeQuery.QueryLowProductSupplierStockItems(storeId, dbc);
            // Converts Entity object to DTO object and adds to the result list.
            result.AddRange(query.Select(EntryObject.ToProductSupplierStockItemDTO));
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
            var query = _storeQuery.QueryProductSuppliers(storeId, dbc);
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
            var query = _storeQuery.QueryAllProductSupplierStockItems(storeId, dbc);
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
            var query = _storeQuery.QueryProductOrderById(productOrderId, dbc);
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

    public IList<ProductOrderDTO> GetAllProductOrders(long storeId)
    {
        List<ProductOrderDTO> result = new();
        using var dbc = new DatabaseContext();
        using var transaction = dbc.Database.BeginTransaction();

        try
        {
            // Makes the query to the database.
            var query = _storeQuery.QueryAllProductOrders(storeId, dbc);
            // Converts Entity object to DTO object and adds to the result list.
            result.AddRange(query.Select(EntryObject.ToProductOrderDTO));
            transaction.Commit();
        }
        catch (ItemNotFoundException e)
        {
            Console.WriteLine(e);
            throw new EnterpriseException("Product orders could not be found!");
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
            var result = _storeQuery.QueryProductOrderById(productOrder.ProductOrderId, dbc);

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
                var item = _storeQuery.QueryProductStockItem(storeId, oe.Product.Barcode, dbc);
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
            var result = _storeQuery.QueryProductStockItemById(stockItemId, dbc);
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
                var query = _storeQuery.QueryProductStockItemById(product.StockItem.ItemId, dbc);
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
            var query = _storeQuery.QueryProductStockItem(storeId, productBarcode, dbc);
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
