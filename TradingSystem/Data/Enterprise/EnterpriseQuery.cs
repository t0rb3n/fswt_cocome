using Microsoft.EntityFrameworkCore;

namespace Data.Enterprise;

public class EnterpriseQuery : IEnterpriseQuery
{
    public Enterprise QueryEnterpriseById(long enterpriseId, DatabaseContext dbc)
    {
        var result = dbc
                         .Enterprises
                         .Find(enterpriseId)
                     ?? throw new Exception($"Can't find enterprise by id {enterpriseId}");
        return result;
    }

    public IList<Store.Store> QueryStores(long enterpriseId, DatabaseContext dbc)
    {
        var result = dbc
                .Stores
                .Where(store => store.Enterprise.Id == enterpriseId)
                .ToList() 
                     ?? throw new Exception($"Can't find stores from enterprise id {enterpriseId}");
        return result;
    }

    public IList<ProductSupplier> QueryProductSuppliers(long enterpriseId, DatabaseContext dbc)
    {
        var result = dbc
                         .Enterprises
                         .Find(enterpriseId)
                         
                     ?? throw new Exception($"Can't find product supplier from enterprise id {enterpriseId}");

        dbc.Entry(result).Collection(enterprise => enterprise.ProductSuppliers).Load();
        
        return result.ProductSuppliers;
    }
    
    public long QueryMeanTimeToDelivery(ProductSupplier productSupplier, Enterprise enterprise, DatabaseContext dbc)
    {
        var result = dbc
            .ProductOrders
            .Where(order => 
                order.DeliveryDate != DateTime.MinValue 
                && order.OrderEntries.Any(e => 
                    e.Product.ProductSupplier.Id == productSupplier.Id)
                && order.Store.Enterprise.Id == enterprise.Id)
            .ToList();

        long meanTime = 0;
        
        foreach (var order in result)
        {
            meanTime += order.DeliveryDate.Ticks - order.OrderingDate.Ticks;
        }
        if (meanTime != 0) meanTime /= result.Count;

        return meanTime;

    }
}