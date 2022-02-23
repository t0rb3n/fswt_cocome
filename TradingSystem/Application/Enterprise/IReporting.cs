namespace Application.Enterprise;

/// <summary>
/// The interface IReporting provides report methods for the web enterprise application component.
/// </summary>
public interface IReporting

{
    /// <summary>
    /// Creates report of available stocks in the specified store.
    /// </summary>
    /// <param name="storeId">The store id for which report should be created.</param>
    /// <returns>A <see cref="StoreStockReportDTO"/> containing store and stock information.</returns>
    public StoreStockReportDTO GetStoreStockReport(long storeId);
    
    /// <summary>
    /// Creates report of all available stocks of specified enterprise.
    /// </summary>
    /// <param name="enterpriseId">The enterprise id for which the report should be created.</param>
    /// <returns>A <see cref="EnterpriseStockReportDTO"/> containing the enterprise
    /// and a list of <see cref="StoreStockReportDTO"/></returns>
    public EnterpriseStockReportDTO GetEnterpriseStockReport(long enterpriseId);
    
    /// <summary>
    /// Creates a report on the mean delivery time for each supplier of the specified enterprise.
    /// </summary>
    /// <param name="enterpriseId">The enterprise id for which the report should be created.</param>
    /// <returns>A list of <see cref="SupplierMeanTimeReportDTO"/> containing the supplier
    /// and the mean delivery time.</returns>
    public IList<SupplierMeanTimeReportDTO> GetMeanTimeToDeliveryReport(long enterpriseId);
}