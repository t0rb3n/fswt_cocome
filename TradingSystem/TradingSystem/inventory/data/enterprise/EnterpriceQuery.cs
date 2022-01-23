namespace TradingSystem.inventory.data.enterprise;

public class EnterpriceQuery : IEnterpriseQuery
{
    private readonly DataContext _database;

    public EnterpriceQuery(DataContext db)
    {
        _database = db;
    }
    public Enterprise QueryEnterpriseById(long enterpriseId)
    {
        throw new NotImplementedException();
    }

    public long QueryMeanTimeToDelivery(ProductSupplier supplier, Enterprise enterprise)
    {
        throw new NotImplementedException();
    }
}