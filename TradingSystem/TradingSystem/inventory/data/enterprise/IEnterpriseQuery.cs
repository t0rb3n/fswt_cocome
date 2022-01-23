namespace TradingSystem.inventory.data.enterprise;

public interface IEnterpriseQuery
{
    public Enterprise QueryEnterpriseById(long enterpriseId);
    public long QueryMeanTimeToDelivery(ProductSupplier supplier, Enterprise enterprise);
}