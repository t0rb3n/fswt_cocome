using TradingSystem.inventory.data.enterprise;
using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.data;

public class Data : IData
{
    public IEnterpriseQuery getIEnterpriseQuery()
    {
        return new EnterpriceQuery();
    }

    public IStoreQuery getIStoreQuery()
    {
        return new StoreQuery();
    }
}