namespace Application.Enterprise;

/// <summary>
/// Class <c>SupplierMeanTimeReportDTO</c> is used as a data transfer object for transferring data from the database.
/// It is used in the Enterprise and Store application to handle the business logic.
/// A DTO can be either a copy of the persisted data for further processing, or
/// to modify or add new data in the persistence layer.
/// </summary>
public class SupplierMeanTimeReportDTO
{
    protected long supplierId;
    protected string supplierName;
    protected TimeSpan meanTime;

    /// <summary>
    /// This constructor initializes the new SupplierMeanTimeReportDTO with default values.
    /// <para>SupplierMeanTimeReportDTO objects with Id = -1 means that it does not contain any data.</para>
    /// </summary>
    public SupplierMeanTimeReportDTO()
    {
        supplierId = -1;
        supplierName = "";
        meanTime = TimeSpan.MinValue;
    }

    /// <summary>
    /// Provides get and set methods for SupplierId property.
    /// </summary>
    /// <value>Property <c>SupplierId</c> represents a unique identifier for the supplier.</value>
    public long SupplierId
    {
        get => supplierId;
        set => supplierId = value;
    }

    /// <summary>
    /// Provides get and set methods for SupplierName property.
    /// </summary>
    /// <value>Property <c>SupplierName</c> represents the name of the supplier.</value>
    public string SupplierName
    {
        get => supplierName;
        set => supplierName = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    /// <summary>
    /// Provides get and set methods for MeanTime property.
    /// </summary>
    /// <value>Property <c>MeanTime</c> represents the mean time delivery of the supplier.</value>
    public TimeSpan MeanTime
    {
        get => meanTime;
        set => meanTime = value;
    }
    
    /// <summary>
    /// Reports a SupplierMeanTimeReportDTO properties as a string.
    /// </summary>
    /// <returns>A string with supplierId, supplierName and
    /// mean time properties of SupplierMeanTimeReportDTO.</returns>
    public override string ToString()
    {
        return $"Id: {supplierId}, Supplier: {supplierName}, MeanTime: {meanTime}";
    }
}