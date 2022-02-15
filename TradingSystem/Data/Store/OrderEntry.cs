using System.ComponentModel.DataAnnotations;
using Data.Enterprise;

namespace Data.Store;

/// <summary>
/// Class <c>OrderEntry</c> represents a single product order entry in the database
/// </summary>
public class OrderEntry
{
    private long _id;
    private int _amount;
    private Product _product;
    private ProductOrder _productOrder;

    /// <summary>
    /// This constructor initializes the new OrderEntry with default values.
    /// <para>OrderEntry objects with Id = -1 means that it does not contain any data.</para>
    /// </summary>
    public OrderEntry()
    {
        _id = -1;
        _amount = 0;
        _product = new Product();
        _productOrder = new ProductOrder();
    }

    /// <summary>
    /// Provides get and set methods for Id property.
    /// </summary>
    /// <value>Property <c>Id</c> represents a unique identifier for OrderEntry objects.</value>
    [Key]
    public long Id
    {
        get => _id;
        set => _id = value;
    }

    /// <summary>
    /// Provides get and set methods for Amount property.
    /// </summary>
    /// <value>Property <c>Amount</c> represents the amount of ordered products.</value>
    public int Amount
    {
        get => _amount;
        set => _amount = value;
    }

    /// <summary>
    /// Provides get and set methods for Product property.
    /// </summary>
    /// <value>Property <c>Product</c> represents the product which is ordered.</value>
    /// <exception cref="ArgumentNullException">If set Product with null.</exception>
    public Product Product
    {
        get => _product;
        set => _product = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Provides get and set methods for ProductOrder property.
    /// </summary>
    /// <value>Property <c>ProductOrder</c> represents where the OrderEntry belongs to.</value>
    /// <exception cref="ArgumentNullException">If set ProductOrder with null.</exception>
    public ProductOrder ProductOrder
    {
        get => _productOrder;
        set => _productOrder = value ?? throw new ArgumentNullException(nameof(value));
    }
}