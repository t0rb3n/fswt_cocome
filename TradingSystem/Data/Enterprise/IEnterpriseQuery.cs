namespace Data.Enterprise;

public interface IEnterpriseQuery
{
    public Enterprise QueryEnterpriseById(long enterpriseId, DatabaseContext dbc);
    public IList<Store.Store> QueryStores(long enterpriseId, DatabaseContext dbc);
    public IList<ProductSupplier> QueryProductSuppliers(long enterpriseId, DatabaseContext dbc);
    public long QueryMeanTimeToDelivery(ProductSupplier productSupplier, Enterprise enterprise, DatabaseContext dbc);
}