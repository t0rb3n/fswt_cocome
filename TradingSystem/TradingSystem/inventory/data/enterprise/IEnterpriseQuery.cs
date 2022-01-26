namespace TradingSystem.inventory.data.enterprise;

public interface IEnterpriseQuery
{
    public Enterprise QueryEnterpriseById(long enterpriseId, DatabaseContext dbc);
    public long QueryMeanTimeToDelivery(ProductSupplier productSupplier, Enterprise enterprise, DatabaseContext dbc);
}