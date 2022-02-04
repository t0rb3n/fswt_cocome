using Data.Store;
using Data.Enterprise;

namespace Data;

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