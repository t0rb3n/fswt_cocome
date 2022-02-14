using System.ComponentModel.DataAnnotations;

namespace Data.Enterprise;

/// <summary>
/// Class <c>Product</c> represents a product in the database.
/// </summary>
public class Product
{
    private long _id;
    private long _barcode;
    private double _purchasePrice;
    private string _name;
    private ProductSupplier _productSupplier;

    /// <summary>
    /// This constructor initializes the new Product with default values.
    /// <para>Product objects with Id = -1 means that it does not contain any data.</para>
    /// </summary>
    public Product()
    {
        _id = -1;
        _barcode = 0;
        _purchasePrice = 0;
        _name = "";
        _productSupplier = new ProductSupplier();
    }
    
    /// <value>Property <c>Id</c> represents a unique identifier for Product objects.</value>
    [Key]
    public long Id
    {
        get => _id;
        set => _id = value;
    }
    
    /// <value>Property <c>Barcode</c> represents the barcode of the product.</value>
    public long Barcode
    {
        get => _barcode;
        set => _barcode = value;
    }

    /// <value>Property <c>PurchasePrice</c> represents the purchase price of this product.</value>
    public double PurchasePrice
    {
        get => _purchasePrice;
        set => _purchasePrice = value;
    }

    /// <value>Property <c>Name</c> represents the name of the product.</value>
    /// <exception cref="ArgumentNullException">If set Name with null.</exception>
    public string Name
    {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <value>Property <c>ProductSupplier</c> represents the supplier of the product.</value>
    /// <exception cref="ArgumentNullException">If set ProductSupplier with null.</exception>
    public ProductSupplier ProductSupplier
    {
        get => _productSupplier;
        set => _productSupplier = value ?? throw new ArgumentNullException(nameof(value));
    }
}