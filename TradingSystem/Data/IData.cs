using Data.Store;
using Data.Enterprise;

namespace Data;
/// <summary>
/// Interface for the Data component
/// </summary>
public interface IData
{
    /// <summary>
    /// creates a new EnterpriseQuery component
    /// </summary>
    /// <returns>new EnterpriseQuery component</returns>
    public IEnterpriseQuery GetEnterpriseQuery();
    /// <summary>
    /// creates a new StoreQuery component
    /// </summary>
    /// <returns>new StoreQuery component</returns>
    public IStoreQuery GetStoreQuery();
}