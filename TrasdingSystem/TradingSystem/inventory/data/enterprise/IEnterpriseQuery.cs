namespace TradingSystem.inventory.data.enterprise;

public interface IEnterpriseQuery
{
    public Enterprise queryEnterpriseById(long enterpriseId);
    public long queryMeanTimeToDelivery(ProductSupplier supplier, Enterprise enterprise);
}