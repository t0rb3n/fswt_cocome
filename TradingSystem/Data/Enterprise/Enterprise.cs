using System.ComponentModel.DataAnnotations;

namespace Data.Enterprise;

/// <summary>
/// Class <c>Enterprise</c> represents an enterprise in the database.
/// </summary>
public class Enterprise
{
    private long _id;
    private string _name;
    private List<ProductSupplier> _productSuppliers;
    private List<Store.Store> _stores;

    /// <summary>
    /// This constructor initializes the new Enterprise with default values.
    /// <para>Enterprise objects with Id = -1 means that it does not contain any data.</para>
    /// </summary>
    public Enterprise()
    {
        _id = -1;
        _name = "";
        _productSuppliers = new List<ProductSupplier>();
        _stores = new List<Store.Store>();
    }

    /// <summary>
    /// Provides get and set methods for Id property.
    /// </summary>
    /// <value>Property <c>Id</c> represents a unique identifier for Enterprise objects.</value>
    [Key]
    public long Id
    {
        get => _id;
        set => _id = value;
    }

    /// <summary>
    /// Provides get and set methods for Name property.
    /// </summary>
    /// <value>Property <c>Name</c> represents the name of the enterprise.</value>
    /// <exception cref="ArgumentNullException">If set Name with null.</exception>
    public string Name
    {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Provides get and set methods for ProductSuppliers property.
    /// </summary>
    /// <value>Property <c>ProductSuppliers</c> represents a list of suppliers related to the enterprise.</value>
    /// <exception cref="ArgumentNullException">If set ProductSuppliers with null.</exception>
    public List<ProductSupplier> ProductSuppliers
    {
        get => _productSuppliers;
        set => _productSuppliers = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Provides get and set methods for Stores property.
    /// </summary>
    /// <value>Property <c>Stores</c> represents a list of stores related to the enterprise.</value>
    /// <exception cref="ArgumentNullException">If set Stores with null.</exception>
    public List<Store.Store> Stores
    {
        get => _stores;
        set => _stores = value ?? throw new ArgumentNullException(nameof(value));
    }
}