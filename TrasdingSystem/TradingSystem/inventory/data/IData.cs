using TradingSystem.inventory.data.enterprise;
using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.data;

public interface IData
{
    public IEnterpriseQuery getIEnterpriseQuery();
    public IStoreQuery getIStoreQuery();
}