using TradingSystem.inventory.data.enterprise;
using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.data;

public class Data : IData
{
    private readonly DataContext _db;

    public Data(DataContext db)
    {
        _db = db;
    }
    public IEnterpriseQuery GetEnterpriseQuery()
    {
        return new EnterpriceQuery(_db);
    }

    public IStoreQuery GetStoreQuery()
    {
        return new StoreQuery(_db);
    }
}