using Application.Exceptions;
using Application.Mappers;
using Application.Store;
using Data;
using Data.Enterprise;
using Data.Exceptions;
using Data.Store;
using Microsoft.Extensions.Logging;

namespace Application.Enterprise;

/// <summary>
/// Class <c>EnterpriseApplication</c> implemented the interfaces of IEnterpriseApplication and IReporting.
/// </summary>
public class EnterpriseApplication : IEnterpriseApplication, IReporting
{
    private readonly IStoreQuery _storeQuery;
    private readonly IEnterpriseQuery _enterpriseQuery;
    private readonly ILogger<EnterpriseApplication> _logger;
    private readonly long _enterpriseId;
    private readonly string _connectionString;

    /// <summary>
    /// This constructor initializes a new enterprise application.
    /// </summary>
    /// <param name="enterpriseQuery">For enterprise queries in the database.</param>
    /// <param name="storeQuery">For store queries in the database.</param>
    /// <param name="logger">Logger for enterprise application events.</param>
    /// <param name="enterpriseId">The id of your enterprise.</param>
    /// <param name="connectionString">Connection string for the database.</param>
    public EnterpriseApplication(IEnterpriseQuery enterpriseQuery, IStoreQuery storeQuery, 
        ILogger<EnterpriseApplication> logger, long enterpriseId, string connectionString)
    {
        _enterpriseQuery = enterpriseQuery;
        _storeQuery = storeQuery;
        _logger = logger;
        _enterpriseId = enterpriseId;
        _connectionString = connectionString;
    }

    public EnterpriseDTO GetEnterprise()
    {
        EnterpriseDTO result;
        using var dbc = new DatabaseContext(_connectionString);
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
            _logger.LogError("EnterpriseApplication: {msg}.", e.Message);
            throw new EnterpriseException("Enterprise could not be found!");
        }
        
        _logger.LogInformation("GetEnterprise: Has received ({name}) enterprise information from the database.",
            result.EnterpriseName);

        return result;
    }

    public IList<StoreDTO> GetEnterpriseStores()
    {
        List<StoreDTO> result = new();
        using var dbc = new DatabaseContext(_connectionString);
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
            _logger.LogError("EnterpriseApplication: {msg}.", e.Message);
            throw new EnterpriseException(
                "An unexpected error occurred while receiving the store list!");
        }
        
        _logger.LogInformation("GetEnterpriseStores: Has received {size} stores information from the database.",
            result.Count);

        return result;
    }
    
    public IList<ProductSupplierDTO> GetEnterpriseProductSuppliers()
    {
        List<ProductSupplierDTO> result = new();
        using var dbc = new DatabaseContext(_connectionString);
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
            _logger.LogError("EnterpriseApplication: {msg}.", e.Message);
            throw new EnterpriseException(
                "An unexpected error occurred while receiving the product supplier list!");
        }
        
        _logger.LogInformation("GetEnterpriseProductSuppliers: Has received {size} supplier information from the database.",
            result.Count);

        return result;
    }

    public StoreStockReportDTO GetStoreStockReport(long storeId)
    {
        StoreStockReportDTO result;

        using var dbc = new DatabaseContext(_connectionString);
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
            result.StockItems.AddRange(stockitems.Select(EntryObject.ToProductSupplierStockItemDTO));
            
            transaction.Commit();
        }
        catch (ItemNotFoundException e)
        {
            _logger.LogError("EnterpriseApplication: {msg}.", e.Message);
            throw new EnterpriseException(
                "An unexpected error occurred while creating the store stock report!");
        }
        
        _logger.LogInformation("GetStoreStockReport: Report created on store {name}.", result.StoreName);

        return result;
    }

    public EnterpriseStockReportDTO GetEnterpriseStockReport(long enterpriseId)
    {
        EnterpriseStockReportDTO result;

        using var dbc = new DatabaseContext(_connectionString);
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
            
            transaction.Commit();
        }
        catch (ItemNotFoundException e)
        {
            _logger.LogError("EnterpriseApplication: {msg}", e.Message);
            throw new EnterpriseException(
                "An unexpected error occurred while creating the enterprise stock report!");
        }
        
        _logger.LogInformation("GetEnterpriseStockReport: Report created on enterprise {name}.", 
            result.EnterpriseName);

        return result;
    }

    public IList<SupplierMeanTimeReportDTO> GetMeanTimeToDeliveryReport(long enterpriseId)
    {
        var result = new List<SupplierMeanTimeReportDTO>();
        
        using var dbc = new DatabaseContext(_connectionString);
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
            _logger.LogError("EnterpriseApplication: {msg}.", e.Message);
            throw new EnterpriseException(
                "An unexpected error occurred while calculating the mean delivery time of a supplier!");
        }

        _logger.LogInformation("GetMeanTimeToDeliveryReport: Report created on {size} product supplier.", 
            result.Count);
        
        return result;
    }

    public StoreEnterpriseDTO GetStoreEnterprise(long storeId)
    {
        StoreEnterpriseDTO result;
        using var dbc = new DatabaseContext(_connectionString);
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
            _logger.LogError("EnterpriseApplication: {msg}.", e.Message);
            throw new EnterpriseException("Store could not be found!");
        }
        
        _logger.LogInformation("GetStoreEnterprise: Has received ({name}) store information from the database.",
            result.StoreName);

        return result;
    }

    public IList<ProductSupplierStockItemDTO> GetLowProductSupplierStockItems(long storeId)
    {
        List<ProductSupplierStockItemDTO> result = new();
        using var dbc = new DatabaseContext(_connectionString);
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
            _logger.LogError("EnterpriseApplication: {msg}.", e.Message);
            throw new EnterpriseException(
                "An unexpected error occurred while receiving the product stock item list!");
        }
        
        _logger.LogInformation("GetLowProductSupplierStockItems: Has received {size} ProductSupplierStockItemDTO from the database.",
            result.Count);

        return result;
    }

    public IList<ProductSupplierDTO> GetAllProductSuppliers(long storeId)
    {
        List<ProductSupplierDTO> result = new();
        using var dbc = new DatabaseContext(_connectionString);
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
            _logger.LogError("EnterpriseApplication: {msg}.", e.Message);
            throw new EnterpriseException(
                "An unexpected error occurred while receiving the product supplier list!");
        }
        
        _logger.LogInformation("GetAllProductSuppliers: Has received {size} ProductSupplierDTO from the database.",
            result.Count);

        return result;
    }

    public IList<ProductSupplierStockItemDTO> GetAllProductSupplierStockItems(long storeId)
    {
        List<ProductSupplierStockItemDTO> result = new();
        using var dbc = new DatabaseContext(_connectionString);
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
            _logger.LogError("EnterpriseApplication: {msg}.", e.Message);
            throw new EnterpriseException(
                "An unexpected error occurred while receiving the product supplier stock item list!");
        }
        
        _logger.LogInformation("GetAllProductSupplierStockItems: Has received {size} ProductSupplierStockItemDTO from the database.",
            result.Count);
        
        return result;
    }

    public void OrderProducts(ProductOrderDTO productOrder, long storeId)
    {
        using var dbc = new DatabaseContext(_connectionString);
        using var transaction = dbc.Database.BeginTransaction();
        
        try
        {
            var poe = new ProductOrder {Id = 0};
            if (productOrder.Orders.Count == 0)
            {
                throw new EnterpriseException("Product order contains no order entries!");
            }
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
            if (productOrder.OrderingDate == DateTime.MinValue)
            {
                throw new EnterpriseException("Order date was not set!");
            }
            // Converts nanosecond of order date in microsecond for the postgresql database
            poe.OrderingDate = new DateTime(productOrder.OrderingDate.Ticks / 10 * 10).ToUniversalTime();
            // Adds product order to the database.
            dbc.ProductOrders.Add(poe);
            // Commits product order to the database.
            dbc.SaveChanges();
            transaction.Commit();
            
            _logger.LogInformation("OrderProducts: Added new product order with {0} items from {1} to database.",
                poe.OrderEntries.Count, poe.OrderingDate);
        }
        catch (ItemNotFoundException e)
        {
            _logger.LogError("EnterpriseApplication: {msg}", e.Message);
            throw new EnterpriseException(
                "An unexpected error occurred while executing the order!");
        }
    }

    public ProductOrderDTO GetProductOrder(long productOrderId)
    {
        ProductOrderDTO result;
        using var dbc = new DatabaseContext(_connectionString);
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
            _logger.LogError("EnterpriseApplication: {msg}.", e.Message);
            throw new EnterpriseException("Product order could not be found!");
        }
        
        _logger.LogInformation("GetProductOrder: Has received product order {id} ProductOrderDTO from the database.",
            productOrderId);
        
        return result;
    }

    public IList<ProductOrderDTO> GetAllProductOrders(long storeId)
    {
        List<ProductOrderDTO> result = new();
        using var dbc = new DatabaseContext(_connectionString);
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
            _logger.LogError("EnterpriseApplication: {msg}.", e.Message);
            throw new EnterpriseException("Product orders could not be found!");
        }
        
        _logger.LogInformation("GetAllProductOrders: Has received {size} ProductOrderDTO from the database.",
            result.Count);
        
        return result;
    }
    
    public IList<ProductOrderDTO> GetAllOpenProductOrders(long storeId)
    {
        List<ProductOrderDTO> result = new();
        using var dbc = new DatabaseContext(_connectionString);
        using var transaction = dbc.Database.BeginTransaction();

        try
        {
            // Makes the query to the database.
            var query = _storeQuery.QueryAllOpenProductOrders(storeId, dbc);
            // Converts Entity object to DTO object and adds to the result list.
            result.AddRange(query.Select(EntryObject.ToProductOrderDTO));
            transaction.Commit();
        }
        catch (ItemNotFoundException e)
        {
            _logger.LogError("EnterpriseApplication: {msg}", e.Message);
            throw new EnterpriseException("Open product orders could not be found!");
        }
        
        _logger.LogInformation("GetAllOpenProductOrders: Has received {size} ProductOrderDTO from the database.",
            result.Count);
        
        return result;
    }

    public void RollInReceivedProductOrder(ProductOrderDTO productOrder, long storeId)
    {
        using var dbc = new DatabaseContext(_connectionString);
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
            if (!dbc.Stores.Any(store => store.Id == storeId))
            {
                throw new EnterpriseException("Product order can not be executed from this store!");
            }
            
            // Sets the delivery date and
            // Converts nanosecond of delivery date in microsecond for the postgresql database
            result.DeliveryDate = new DateTime(productOrder.DeliveryDate.Ticks / 10 * 10).ToUniversalTime();

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
            _logger.LogError("EnterpriseApplication: {msg}.", e.Message);
            throw new EnterpriseException("Product order could not be found!");
        }
        
        _logger.LogInformation("RollInReceivedProductOrder: Product order {id} has been completed and stock replenished.",
            productOrder.ProductOrderId);
    }

    public void ChangePrice(long stockItemId, double newPrice)
    {
        using var dbc = new DatabaseContext(_connectionString);
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
            _logger.LogError("EnterpriseApplication: {msg}.", e.Message);
            throw new EnterpriseException("An unexpected error occurred while changing the price!");
        }
        
        _logger.LogInformation("ChangePrice: The new price for the stock item {id} has been set in the database.",
            stockItemId);
    }
    
    public void MakeBookSale(SaleDTO saleDto)
    {
        using var dbc = new DatabaseContext(_connectionString);
        using var transaction = dbc.Database.BeginTransaction();

        try
        {
            if (saleDto.Products.Count == 0)
            {
                throw new EnterpriseException("The sale does not include product sales!");
            }
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
            _logger.LogError("EnterpriseApplication: {msg}.", e.Message);
            throw new EnterpriseException("An unexpected error occurred while booking the stock items!");
        }
        
        _logger.LogInformation("MakeBookSale: Has booked the sale {date} from the store server.",
            saleDto.Date);
    }

    public ProductStockItemDTO GetProductStockItem(long productBarcode, long storeId)
    {
        ProductStockItemDTO result;
        using var dbc = new DatabaseContext(_connectionString);
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
            _logger.LogError("EnterpriseApplication: {msg}.", e.Message);
            throw new EnterpriseException("Product stock item could not be found!");
        }

        _logger.LogInformation("GetProductStockItem: Has received ProductStockItemDTO ({barcode}) from the database.",
            productBarcode);
        
        return result;
    }
}
