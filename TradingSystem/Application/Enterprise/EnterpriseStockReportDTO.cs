namespace Application.Enterprise;

/// <summary>
/// Class <c>EnterpriseStockReportDTO</c> is used as a data transfer object for transferring data from the database.
/// It is used in the Enterprise and Store application to handle the business logic.
/// A DTO can be either a copy of the persisted data for further processing, or
/// to modify or add new data in the persistence layer.
/// </summary>
public class EnterpriseStockReportDTO : EnterpriseDTO
{
    protected List<StoreStockReportDTO> storeReports;

    /// <summary>
    /// This constructor initializes the new EnterpriseStockReportDTO with default values.
    /// <para>EnterpriseStockReportDTO objects with Id = -1 means that it does not contain any data.</para>
    /// </summary>
    public EnterpriseStockReportDTO()
    {
        enterpriseId = -1;
        enterpriseName = "";
        storeReports = new List<StoreStockReportDTO>();
    }

    /// <summary>
    /// Provides get and set methods for StoreReports property.
    /// </summary>
    /// <value>Property <c>StoreReports</c> represents a list of <see cref="StoreStockReportDTO"/>.</value>
    /// <exception cref="ArgumentNullException">If set StoreReports with null.</exception>
    public List<StoreStockReportDTO> StoreReports
    {
        get => storeReports;
        set => storeReports = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    /// <summary>
    /// Reports a EnterpriseStockReportDTO properties as a string.
    /// </summary>
    /// <returns>A string with enterpriseId, enterpriseName enterprise properties and
    /// a list of its stores' inventories.</returns>
    public override string ToString()
    {
        var printList = "";
        foreach (var store in StoreReports)
        {
            printList += $"\t{store}\n";
        }
        return $"Id: {enterpriseId}, Enterprise: {enterpriseName}\n\n{printList}";
    }
    
    /// <summary>
    /// This method determines whether two EnterpriseStockReportDTO have the same properties.
    /// </summary>
    /// <param name="obj">Is the object to be compared to the current object.</param>
    /// <returns>True if EnterpriseStockReportDTO are equals otherwise false.</returns>
    public override bool Equals(object? obj)
    {
        var item = obj as EnterpriseStockReportDTO;

        if (item == null)
        {
            return false;
        }

        return enterpriseId.Equals(item.enterpriseId) &&
               enterpriseName.Equals(item.enterpriseName) &&
               storeReports.SequenceEqual(item.storeReports);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(enterpriseId, storeReports);
    }
}