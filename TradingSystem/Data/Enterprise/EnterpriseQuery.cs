using Data.Exceptions;

namespace Data.Enterprise;

/// <summary>
/// Class <c>EnterpriseQuery</c> implemented the interfaces of IEnterpriseQuery
/// </summary>
public class EnterpriseQuery : IEnterpriseQuery
{
    public Enterprise QueryEnterpriseById(long enterpriseId, DatabaseContext dbc)
    {
        Enterprise result;
        
        try
        {
            result = dbc
                .Enterprises
                .Find(enterpriseId)!;
            
            if (result == null)
            {
                throw new ItemNotFoundException($"Enterprise with the id '{enterpriseId}' could not be found!");
            }
        }
        catch (ArgumentNullException)
        {
            throw new ItemNotFoundException($"Enterprise with the id '{enterpriseId}' could not be found!");
        }
      
        return result;
    }

    public IList<Store.Store> QueryStores(long enterpriseId, DatabaseContext dbc)
    {
        List<Store.Store> result;
        try
        {
            result = dbc
                .Stores
                .Where(store => store.Enterprise.Id == enterpriseId)
                .ToList();
            
            if (result.Count == 0)
            {
                throw new ItemNotFoundException($"Stores from enterprise id '{enterpriseId}' could not be found!");
            }
        }
        catch (ArgumentNullException)
        {
            throw new ItemNotFoundException($"Stores from enterprise id '{enterpriseId}' could not be found!");
        }
        
        return result;
    }

    public IList<ProductSupplier> QueryProductSuppliers(long enterpriseId, DatabaseContext dbc)
    {
        Enterprise result;
        try
        {
            result = dbc
                .Enterprises
                .Find(enterpriseId)!;
            
            dbc.Entry(result)
                .Collection(enterprise => enterprise.ProductSuppliers)
                .Load();
            
            if (result.ProductSuppliers.Count == 0)
            {
                throw new ItemNotFoundException($"Product suppliers from enterprise id '{enterpriseId}' could not be found!");
            }
        }
        catch (ArgumentNullException)
        {
            throw new ItemNotFoundException($"Product suppliers from enterprise id '{enterpriseId}' could not be found!");
        }

        return result.ProductSuppliers;
    }
    
    public long QueryMeanTimeToDelivery(long productSupplierId, long enterpriseId, DatabaseContext dbc)
    {
        long meanTime = 0;

        try
        {
            if (!dbc.Enterprises.Any(e => e.Id == enterpriseId))
            {
                throw new ItemNotFoundException($"Enterprise with the id '{enterpriseId}' could not be found!");
            }
            if (!dbc.ProductSuppliers.Any(p => p.Id == productSupplierId))
            {
                throw new ItemNotFoundException($"Suppliers with the id '{productSupplierId}' could not be found!");
            }
            
            var result = dbc
                .ProductOrders
                .Where(order => 
                    order.DeliveryDate != DateTime.MinValue 
                    && order.OrderEntries.Any(e => 
                        e.Product.ProductSupplier.Id == productSupplierId)
                    && order.Store.Enterprise.Id == enterpriseId)
                .ToList();

            foreach (var order in result) meanTime += order.DeliveryDate.Ticks - order.OrderingDate.Ticks;
            
            if (meanTime != 0) meanTime /= result.Count;
            
        }
        catch (ArgumentNullException)
        {
            throw new ItemNotFoundException($"The mean time could not be calculated because an item was not found by the query!");
        }

        return meanTime;
    }
}