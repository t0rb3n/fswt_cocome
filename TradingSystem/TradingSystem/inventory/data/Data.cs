using TradingSystem.inventory.data.enterprise;
using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.data;

public class Data : IData
{
    public IEnterpriseQuery GetEnterpriseQuery()
    {
        return new EnterpriseQuery();
    }

    public IStoreQuery GetStoreQuery()
    {
        return new StoreQuery();
    }
}