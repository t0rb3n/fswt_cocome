using Data;
using Data.Store;
using Grpc.Net.Client;

namespace Application.Store;

public class StoreApplication : IStoreApplication, ICashDeskConnector
{
    private readonly IStoreQuery _storeQuery = IDataFactory.GetInstance().GetStoreQuery();
    private readonly long _storeId;

    public StoreApplication(long storeId)
    {
        _storeId = storeId;
    }

    public StoreEnterpriseDTO GetStore()
    {
        StoreEnterpriseDTO result = new();

        using var dbc = new DatabaseContext();
        using var ctx = dbc.Database.BeginTransaction();
        
        try
        {
            var query = _storeQuery.QueryStoreById(_storeId, dbc);
            result = ConvertEntryObject.ToStoreEnterpriseDTO(query);
            ctx.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return result;
    }

    public List<ProductStockItemDTO> GetProductsLowStockItems()
    {
        List<ProductStockItemDTO> result = new();
        using var dbc = new DatabaseContext();
        using var ctx = dbc.Database.BeginTransaction();

        try
        {
            var query = _storeQuery.QueryLowStockItems(_storeId, dbc);
            result.AddRange(query.Select(ConvertEntryObject.ToProductStockItemDTO));
            ctx.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return result;
    }

    public List<ProductSupplierDTO> GetAllProductSuppliers()
    {
        List<ProductSupplierDTO> result = new();
        using var dbc = new DatabaseContext();
        using var ctx = dbc.Database.BeginTransaction();
        
        try
        {
            var query = _storeQuery.QueryProducts(_storeId, dbc);
            result.AddRange(query.Select(ConvertEntryObject.ToProductSupplierDTO));
            ctx.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return result;
    }

    public List<ProductSupplierStockItemDTO> GetAllProductSupplierStockItems()
    {
        List<ProductSupplierStockItemDTO> result = new();
        using var dbc = new DatabaseContext();
        using var ctx = dbc.Database.BeginTransaction();

        try
        {
            var query = _storeQuery.QueryAllProductStockItems(_storeId, dbc);
            result.AddRange(query.Select(ConvertEntryObject.ToProductSupplierStockItemDTO));
            ctx.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        return result;
    }

    //TODO: refactor creation of ProductOrderEntry foreach Supplier
    public void OrderProducts(ProductOrderDTO productOrderDto)
    {
        ProductOrder poe = new();
        using var dbc = new DatabaseContext();
        using var trans = dbc.Database.BeginTransaction();
        try
        {
            foreach (var order in productOrderDto.Orders)
            {
                var product = _storeQuery.QueryProductById(order.ProductSupplier.ProductId, dbc);
                var oe = new OrderEntry();
                oe.Amount = order.Amount;
                oe.Product = product;
                poe.OrderEntries.Add(oe);
            }

            poe.Store = _storeQuery.QueryStoreById(_storeId, dbc);
            poe.OrderingDate = productOrderDto.OrderingDate;
            dbc.ProductOrders.Attach(poe);
            dbc.SaveChanges();
            trans.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public ProductOrderDTO GetProductOrder(long productOrderId)
    {
        ProductOrderDTO result = new();
        using var dbc = new DatabaseContext();
        using var ctx = dbc.Database.BeginTransaction();

        try
        {
            var query = _storeQuery.QueryOrderById(productOrderId, dbc);
            result = ConvertEntryObject.ToProductOrderDTO(query);
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

            if (result.DeliveryDate == null)
            {
                throw new Exception("Product order has already been received");
            }
            
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

    public void ChangePrice(StockItemDTO stockItem)
    {
        /*using var dbc = new DatabaseContext();
        using var ctx = dbc.Database.BeginTransaction();
        try
        {
            var result = _storeQuery.QueryStockItemById(stockItem.ItemId, dbc);
            result.SalesPrice = stockItem.SalesPrice;
            dbc.SaveChanges();
            ctx.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }*/
        var httpHandler = new HttpClientHandler();
        httpHandler.ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        
        using var channel = GrpcChannel.ForAddress("https://localhost:7046/grpc", new GrpcChannelOptions{HttpHandler = httpHandler});
        var client = new Greeter.GreeterClient(channel);
        var d = new StockItemRequest();
        
            var reply = client.ChangePrice(new StockItemRequest
        {
            ItemId = stockItem.ItemId,
            Amount = stockItem.Amount,
            MaxStock = stockItem.MaxStock,
            MinStock = stockItem.MinStock,
            SalesPrice = stockItem.SalesPrice
        });
        Console.WriteLine("ChangePrice success: " + reply.Success);
        
        
    }

    public void BookSale(SaleDTO saleDto)
    {
        throw new NotImplementedException();
    }

    public ProductStockItemDTO GetProductStockItem(long productBarcode)
    {
        ProductStockItemDTO result = new();
        using var dbc = new DatabaseContext();
        using var ctx = dbc.Database.BeginTransaction();

        try
        {
            var query = _storeQuery.QueryStockItem(_storeId, productBarcode, dbc);
            result = ConvertEntryObject.ToProductStockItemDTO(query);
            ctx.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        return result;
    }
}