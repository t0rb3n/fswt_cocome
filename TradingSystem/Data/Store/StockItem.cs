using System.ComponentModel.DataAnnotations;
using Data.Enterprise;

namespace Data.Store;

/// <summary>
/// Class <c>StockItem</c> represents a concrete product from the store in the database.
/// </summary>
public class StockItem
{
    private long _id;
    private double _salesPrice;
    private int _amount;
    private int _minStock;
    private int _maxStock;
    private Store _store;
    private Product _product;

    /// <summary>
    /// This constructor initializes the new StockItem with default values.
    /// <para>StockItem objects with Id = -1 means that it does not contain any data.</para>
    /// </summary>
    public StockItem()
    {
        _id = -1;
        _salesPrice = 0;
        _amount = 0;
        _minStock = 0;
        _maxStock = 0;
        _store = new Store();
        _product = new Product();
    }
    
    /// <summary>
    /// Provides get and set methods for Id property.
    /// </summary>
    /// <value>Property <c>Id</c> represents a unique identifier for StockItem objects.</value>
    [Key]
    public long Id
    {
        get => _id;
        set => _id = value;
    }

    /// <summary>
    /// Provides get and set methods for SalesPrice property.
    /// </summary>
    /// <value>Property <c>SalesPrice</c> represents the sales price of the StockItem.</value>
    public double SalesPrice
    {
        get => _salesPrice;
        set => _salesPrice = value;
    }

    /// <summary>
    /// Provides get and set methods for Amount property.
    /// </summary>
    /// <value>Property <c>Amount</c> represents the currently available amount of items of a product.</value>
    public int Amount
    {
        get => _amount;
        set => _amount = value;
    }

    /// <summary>
    /// Provides get and set methods for MinStock property.
    /// </summary>
    /// <value>Property <c>MinStock</c> represents the minimum amount of products which has to be available in a store.</value>
    public int MinStock
    {
        get => _minStock;
        set => _minStock = value;
    }

    /// <summary>
    /// Provides get and set methods for MaxStock property.
    /// </summary>
    /// <value>Property <c>MaxStock</c> represents the maximum amount of a product in a store.</value>
    public int MaxStock
    {
        get => _maxStock;
        set => _maxStock = value;
    }
    
    /// <summary>
    /// Provides get and set methods for Store property.
    /// </summary>
    /// <value>Property <c>Store</c> represents the store where the StockItem belongs to.</value>
    /// <exception cref="ArgumentNullException">If set Store with null.</exception>
    public Store Store
    {
        get => _store;
        set => _store = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Provides get and set methods for Product property.
    /// </summary>
    /// <value>Property <c>Product</c> represents the product of a StockItem.</value>
    /// <exception cref="ArgumentNullException">If set Product with null.</exception>
    public Product Product
    {
        get => _product;
        set => _product = value ?? throw new ArgumentNullException(nameof(value));
    }
}