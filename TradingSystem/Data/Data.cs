using Data.Store;
using Data.Enterprise;

namespace Data;

/// <summary>
/// A data instance represents the queries to the database for the enterprise and store application 
/// </summary>
public class Data : IData
{
    /// <summary>
    /// Creates a EnterpriseQuery instance
    /// </summary>
    /// <returns>a new EnterpriseQuery instance</returns>
    public IEnterpriseQuery GetEnterpriseQuery()
    {
        return new EnterpriseQuery();
    }

    /// <summary>
    /// Creates a StoreQuery instance
    /// </summary>
    /// <returns>a new StoreQuery instance</returns>
    public IStoreQuery GetStoreQuery()
    {
        return new StoreQuery();
    }
}