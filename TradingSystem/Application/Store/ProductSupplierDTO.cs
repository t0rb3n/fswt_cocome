namespace Application.Store;

/// <summary>
/// Class <c>ProductSupplierDTO</c> is used as a data transfer object for transferring data from the database.
/// It is used in the Enterprise and Store application to handle the business logic.
/// A DTO can be either a copy of the persisted data for further processing, or
/// to modify or add new data in the persistence layer.
/// </summary>
public class  ProductSupplierDTO : ProductDTO
{
    protected long supplierId;
    protected string supplierName;

    /// <summary>
    /// This constructor initializes the new StoreDTO with default values.
    /// <para>StoreDTO objects with Id = -1 means that it does not contain any data.</para>
    /// </summary>
    public ProductSupplierDTO()
    {
        productId = -1;
        barcode = 0;
        purchasePrice = 0;
        productName = "";
        supplierId = -1;
        supplierName = "";
    }

    /// <summary>
    /// Provides get and set methods for SupplierId property.
    /// </summary>
    /// <value>Property <c>SupplierId</c> represents a unique identifier for ProductSupplier objects.</value>
    public long SupplierId
    {
        get => supplierId;
        set => supplierId = value;
    }

    /// <summary>
    /// Provides get and set methods for SupplierName property.
    /// </summary>
    /// <value>Property <c>SupplierName</c> represents the name of the ProductSupplier objects.</value>
    public string SupplierName
    {
        get => supplierName;
        set => supplierName = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Reports a ProductSupplierDTO properties as a string.
    /// </summary>
    /// <returns>A string with the product supplier properties supplierId, supplierName, productId,
    /// barcode, productName and purchasePrice.</returns>
    public override string ToString()
    {
        return $"Id: {supplierId}, Supplier: {supplierName}\n" +
               $"\tId: {productId}, Barcode: {barcode}, Product: {productName}, purPrice: {purchasePrice.ToString("F2")} €";
    }
    
    /// <summary>
    /// This method determines whether two ProductSupplierDTO have the same properties.
    /// </summary>
    /// <param name="obj">Is the object to be compared to the current object.</param>
    /// <returns>True if ProductSupplierDTO are equals otherwise false.</returns>
    public override bool Equals(object? obj)
    {
        var item = obj as ProductSupplierDTO;
        
        if (item == null)
        {
            return false;
        }

        return productId.Equals(item.productId) &&
               barcode.Equals(item.barcode) &&
               purchasePrice.Equals(item.purchasePrice) &&
               productName.Equals(item.productName) &&
               supplierId.Equals(item.supplierId) &&
               supplierName.Equals(item.supplierName);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(productId, supplierId);
    }
}
