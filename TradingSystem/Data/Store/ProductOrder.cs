using System.ComponentModel.DataAnnotations;

namespace Data.Store;

/// <summary>
/// Class <c>ProductOrder</c> represents a product order of a store in the database.
/// </summary>
public class ProductOrder
{
    private long _id;
    private DateTime _deliveryDate;
    private DateTime _orderingDate;
    private List<OrderEntry> _orderEntries;
    private Store _store;

    /// <summary>
    /// This constructor initializes the new ProductOrder with default values.
    /// <para>ProductOrder objects with Id = -1 means that it does not contain any data.</para>
    /// </summary>
    public ProductOrder()
    {
        _id = -1;
        _orderEntries = new List<OrderEntry>();
        _store = new Store();
    }

    /// <value>Property <c>Id</c> represents a unique identifier for ProductOrder objects.</value>
    [Key]
    public long Id
    {
        get => _id;
        set => _id = value;
    }

    /// <value>Property <c>DeliveryDate</c> represents the delivery date for this order.</value>
    public DateTime DeliveryDate
    {
        get => _deliveryDate;
        set => _deliveryDate = value;
    }

    /// <value>Property <c>DeliveryDate</c> represents the creation date for this order.</value>
    public DateTime OrderingDate
    {
        get => _orderingDate;
        set => _orderingDate = value;
    }

    /// <value>Property <c>OrderEntries</c> represents a list of products that contain this order.</value>
    /// <exception cref="ArgumentNullException">If set OrderEntries with null.</exception>
    public List<OrderEntry> OrderEntries
    {
        get => _orderEntries;
        set => _orderEntries = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <value>Property <c>Store</c> represents the store where the order is placed.</value>
    /// <exception cref="ArgumentNullException">If set Store with null.</exception>
    public Store Store
    {
        get => _store;
        set => _store = value ?? throw new ArgumentNullException(nameof(value));
    }
}