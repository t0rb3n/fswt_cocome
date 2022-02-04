using Data.Store;
using Data.Enterprise;

namespace Data;

public interface IData
{
    public IEnterpriseQuery GetEnterpriseQuery();
    public IStoreQuery GetStoreQuery();
}