using System.ComponentModel.DataAnnotations;

namespace Data.Enterprise;

/// <summary>
/// Class <c>ProductSupplier</c> represents a supplier of the products in the database.
/// </summary>
public class ProductSupplier
{
    private long _id;
    private string _name;
    private List<Product> _products;

    /// <summary>
    /// This constructor initializes the new ProductSupplier with default values.
    /// <para>ProductSupplier objects with Id = -1 means that it does not contain any data.</para>
    /// </summary>
    public ProductSupplier() 
    {
        _id = -1;
        _name = "";
        _products = new List<Product>();
    }
    
    /// <summary>
    /// Provides get and set methods for Id property.
    /// </summary>
    /// <value>Property <c>Id</c> represents a unique identifier for ProductSupplier objects.</value>
    [Key]
    public long Id
    {
        get => _id;
        set => _id = value;
    }

    /// <summary>
    /// Provides get and set methods for Name property.
    /// </summary>
    /// <value>Property <c>Name</c> represents the name of the supplier.</value>
    /// <exception cref="ArgumentNullException">If set Name with null.</exception>
    public string Name
    {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Provides get and set methods for Products property.
    /// </summary>
    /// <value>Property <c>Products</c> represents a list of products provided by the product supplier.</value>
    /// <exception cref="ArgumentNullException">If set Products with null.</exception>
    public List<Product> Products
    {
        get => _products;
        set => _products = value ?? throw new ArgumentNullException(nameof(value));
    }
}