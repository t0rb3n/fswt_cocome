namespace Application.Store;

/// <summary>
/// Class <c>ProductDTO</c> is used as a data transfer object for transferring data from the database.
/// It is used in the Enterprise and Store application to handle the business logic.
/// A DTO can be either a copy of the persisted data for further processing, or
/// to modify or add new data in the persistence layer.
/// </summary>
public class ProductDTO
{
    protected long productId;
    protected long barcode;
    protected double purchasePrice;
    protected string productName;
    
    /// <summary>
    /// This constructor initializes the new ProductDTO with default values.
    /// <para>ProductDTO objects with Id = -1 means that it does not contain any data.</para>
    /// </summary>
    public ProductDTO()
    {
        productId = -1;
        barcode = 0;
        purchasePrice = 0;
        productName = "";
    }

    /// <summary>
    /// Provides get and set methods for ProductId property.
    /// </summary>
    /// <value>Property <c>ProductId</c> represents a unique identifier for Product objects.</value>
    public long ProductId
    {
        get => productId;
        set => productId = value;
    }

    /// <summary>
    /// Provides get and set methods for Barcode property.
    /// </summary>
    /// <value>Property <c>Barcode</c> represents the barcode of the product for Product objects.</value>
    public long Barcode
    {
        get => barcode;
        set => barcode = value;
    }

    /// <summary>
    /// Provides get and set methods for PurchasePrice property.
    /// </summary>
    /// <value>Property <c>PurchasePrice</c> represents the purchase price of the product for Product objects.</value>
    public double PurchasePrice
    {
        get => purchasePrice;
        set => purchasePrice = value;
    }

    /// <summary>
    /// Provides get and set methods for ProductName property.
    /// </summary>
    /// <value>Property <c>ProductName</c> represents the name of the Product objects.</value>
    /// <exception cref="ArgumentNullException">If set ProductName with null.</exception>
    public string ProductName
    {
        get => productName;
        set => productName = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    /// <summary>
    /// Reports a ProductDTO properties as a string.
    /// </summary>
    /// <returns>A string with the product properties productId, barcode, productName and purchasePrice.</returns>
    public override string ToString()
    {
        return $"Id: {productId}, Barcode: {barcode}, Name: {productName}, purPrice: {purchasePrice.ToString("F2")} €";
    }
    
    /// <summary>
    /// This method determines whether two ProductDTO have the same properties.
    /// </summary>
    /// <param name="obj">Is the object to be compared to the current object.</param>
    /// <returns>True if ProductDTO are equals otherwise false.</returns>
    public override bool Equals(object? obj)
    {
        var item = obj as ProductDTO;
        
        if (item == null)
        {
            return false;
        }

        return productId.Equals(item.productId) &&
               barcode.Equals(item.barcode) &&
               purchasePrice.Equals(item.purchasePrice) &&
               productName.Equals(item.productName);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(productId);
    }
}
