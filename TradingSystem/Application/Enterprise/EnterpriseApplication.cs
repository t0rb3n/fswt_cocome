using Application.Store;
using Data;
using Data.Enterprise;
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
        EnterpriseDTO result = new();

        using var dbc = new DatabaseContext();
        using var ctx = dbc.Database.BeginTransaction();
        
        try
        {
            var query = _enterpriseQuery.QueryEnterpriseById(_enterpriseId, dbc);
            result = EntryObject.ToEnterpriseDTO(query);
            ctx.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
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
        throw new NotImplementedException();
    }

    public StoreEnterpriseDTO GetStore(long storeId)
    {
        StoreEnterpriseDTO result = new();

        using var dbc = new DatabaseContext();
        using var ctx = dbc.Database.BeginTransaction();
        
        try
        {
            var query = _storeQuery.QueryStoreById(storeId, dbc);
            result = EntryObject.ToStoreEnterpriseDTO(query);
            ctx.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return result;
    }

    public List<ProductStockItemDTO> GetProductsLowStockItems(long storeId)
    {
        List<ProductStockItemDTO> result = new();
        using var dbc = new DatabaseContext();
        using var ctx = dbc.Database.BeginTransaction();

        try
        {
            var query = _storeQuery.QueryLowStockItems(storeId, dbc);
            result.AddRange(query.Select(EntryObject.ToProductStockItemDTO));
            ctx.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return result;
    }

    public List<ProductSupplierDTO> GetAllProductSuppliers(long storeId)
    {
        List<ProductSupplierDTO> result = new();
        using var dbc = new DatabaseContext();
        using var ctx = dbc.Database.BeginTransaction();
        
        try
        {
            var query = _storeQuery.QueryProducts(storeId, dbc);
            result.AddRange(query.Select(EntryObject.ToProductSupplierDTO));
            ctx.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return result;
    }

    public List<ProductSupplierStockItemDTO> GetAllProductSupplierStockItems(long storeId)
    {
        List<ProductSupplierStockItemDTO> result = new();
        using var dbc = new DatabaseContext();
        using var ctx = dbc.Database.BeginTransaction();

        try
        {
            var query = _storeQuery.QueryAllProductStockItems(storeId, dbc);
            result.AddRange(query.Select(EntryObject.ToProductSupplierStockItemDTO));
            ctx.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        return result;
    }

    public void OrderProducts(ProductOrderDTO productOrder, long storeId)
    {
        using var dbc = new DatabaseContext();
        using var trans = dbc.Database.BeginTransaction();
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
            result = EntryObject.ToProductOrderDTO(query);
            ctx.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        return result;
    }

    public void RollInReceivedProductOrder(ProductOrderDTO productOrder, long storeId)
    {
        using var dbc = new DatabaseContext();
        using var trans = dbc.Database.BeginTransaction();
        try
        {
            var result = _storeQuery.QueryOrderById(productOrder.ProductOrderId, dbc);

            if (result.DeliveryDate != DateTime.UnixEpoch)
            {
                throw new Exception("Product order has already been received");
            }
            
            result.DeliveryDate = productOrder.DeliveryDate;

            foreach (var oe in result.OrderEntries)
            {
                var item = _storeQuery.QueryStockItem(storeId, oe.Product.Barcode, dbc);
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

    public bool ChangePrice(long stockItemId, double newPrice)
    {
        using var dbc = new DatabaseContext();
        using var ctx = dbc.Database.BeginTransaction();
        try
        {
            var result = _storeQuery.QueryStockItemById(stockItemId, dbc);
            result.SalesPrice = newPrice;
            dbc.SaveChanges();
            ctx.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return true;
    }

    public void MakeBookSale(SaleDTO saleDto)
    {
        using var dbc = new DatabaseContext();
        using var ctx = dbc.Database.BeginTransaction();

        try
        {
            foreach (var product in saleDto.Products)
            {
                var query = _storeQuery.QueryStockItemById(product.StockItem.ItemId, dbc);
                query.Amount -= 1;
                dbc.SaveChanges();
            }
            ctx.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public ProductStockItemDTO GetProductStockItem(long productBarcode, long storeId)
    {
        ProductStockItemDTO result = new();
        using var dbc = new DatabaseContext();
        using var ctx = dbc.Database.BeginTransaction();

        try
        {
            var query = _storeQuery.QueryStockItem(storeId, productBarcode, dbc);
            result = EntryObject.ToProductStockItemDTO(query);
            ctx.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        return result;
    }
}