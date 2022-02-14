using System.ComponentModel.DataAnnotations;

namespace Data.Store;

/// <summary>
/// Class <c>Store</c> represents a store in the database.
/// </summary>
public class Store
{
    private long _id;
    private string _name;
    private string _location;
    private Enterprise.Enterprise _enterprise;
    private List<ProductOrder> _productOrders;
    private List<StockItem> _stockItems;

    /// <summary>
    /// This constructor initializes the new Store with default values.
    /// <para>Store objects with Id = -1 means that it does not contain any data.</para>
    /// </summary>
    public Store()
    {
        _id = -1;
        _name = "";
        _location = "";
        _enterprise = new Enterprise.Enterprise();
        _productOrders = new List<ProductOrder>();
        _stockItems = new List<StockItem>();
    }

    /// <value>Property <c>Id</c> represents a unique identifier for Store objects.</value>
    [Key]
    public long Id
    {
        get => _id;
        set => _id = value;
    }
    
    /// <value>Property <c>Name</c> represents the name of the store.</value>
    /// <exception cref="ArgumentNullException">If set Name with null.</exception>
    public string Name
    {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <value>Property <c>Location</c> represents the location of the store.</value>
    /// <exception cref="ArgumentNullException">If set Location with null.</exception>
    public string Location
    {
        get => _location;
        set => _location = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <value>Property <c>Enterprise</c> represents the enterprise which the store belongs to.</value>
    /// <exception cref="ArgumentNullException">If set Enterprise with null.</exception>
    public Enterprise.Enterprise Enterprise
    {
        get => _enterprise;
        set => _enterprise = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <value>Property <c>ProductOrders</c> represents a list of product orders from the store.</value>
    /// <exception cref="ArgumentNullException">If set ProductOrders with null.</exception>
    public List<ProductOrder> ProductOrders
    {
        get => _productOrders;
        set => _productOrders = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <value>Property <c>StockItems</c> represents a list of stock items from the store.</value>
    /// <exception cref="ArgumentNullException">If set StockItems with null.</exception>
    public List<StockItem> StockItems
    {
        get => _stockItems;
        set => _stockItems = value ?? throw new ArgumentNullException(nameof(value));
    }
}