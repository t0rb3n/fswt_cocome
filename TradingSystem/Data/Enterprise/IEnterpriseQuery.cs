using Data.Exceptions;

namespace Data.Enterprise;

/// <summary>
/// Interface IEnterpriseQuery provides methods for querying the database.
/// </summary>
public interface IEnterpriseQuery
{
    /// <summary>
    /// Queries for a specified enterprise
    /// </summary>
    /// <param name="enterpriseId">A unique id of the enterprise.</param>
    /// <param name="dbc">A database context to query.</param>
    /// <returns>A <see cref="Enterprise"/> which has this id.</returns>
    /// <exception cref="ItemNotFoundException">If no enterprise was found.</exception>
    public Enterprise QueryEnterpriseById(long enterpriseId, DatabaseContext dbc);
    
    /// <summary>
    /// Queries for all stores that a enterprise has and adds the store reference.
    /// </summary>
    /// <param name="enterpriseId">A unique id of the enterprise.</param>
    /// <param name="dbc">A database context to query.</param>
    /// <returns>A list of <see cref="Store"/> by this enterprise</returns>
    /// <exception cref="ItemNotFoundException">If no stores was found by this enterprise</exception>
    public IList<Store.Store> QueryStores(long enterpriseId, DatabaseContext dbc);
    
    /// <summary>
    /// Queries for all product suppliers that a enterprise has and adds the store reference.
    /// </summary>
    /// <param name="enterpriseId">A unique id of the enterprise.</param>
    /// <param name="dbc">A database context to query.</param>
    /// <returns>A list of <see cref="ProductSupplier"/> by this enterprise</returns>
    /// <exception cref="ItemNotFoundException">If no product suppliers was found by this enterprise</exception>
    public IList<ProductSupplier> QueryProductSuppliers(long enterpriseId, DatabaseContext dbc);
    
    /// <summary>
    /// Queries the mean time required by a supplier in this enterprise.
    /// </summary>
    /// <param name="productSupplierId">The supplier id which delivers the products</param>
    /// <param name="enterpriseId">The enterprise id for which the products are delivered</param>
    /// <param name="dbc">A database context to query.</param>
    /// <returns>The mean time in milliseconds or 0 if the supplier has not yet delivered.</returns>
    /// <exception cref="ItemNotFoundException">If no supplier or enterprise was found or by the query.</exception>
    public long QueryMeanTimeToDelivery(long productSupplierId, long enterpriseId, DatabaseContext dbc);
}