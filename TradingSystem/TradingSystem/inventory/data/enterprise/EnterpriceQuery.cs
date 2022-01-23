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
        var result = _database
                         .Enterprises
                         .Find(enterpriseId) 
                     ?? throw new ArgumentException($"Can't find enterprise by id {enterpriseId}");
        return result;
    }

    public long QueryMeanTimeToDelivery(ProductSupplier supplier, Enterprise enterprise)
    {
        throw new NotImplementedException();
    }
}