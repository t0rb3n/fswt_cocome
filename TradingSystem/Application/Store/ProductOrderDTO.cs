namespace Application.Store;

/// <summary>
/// Class <c>ProductOrderDTO</c> is used as a data transfer object for transferring data from the database.
/// It is used in the Enterprise and Store application to handle the business logic.
/// A DTO can be either a copy of the persisted data for further processing, or
/// to modify or add new data in the persistence layer.
/// </summary>
public class ProductOrderDTO
{
    protected long productOrderId;
    protected DateTime deliveryDate;
    protected DateTime orderingDate;
    protected List<OrderDTO> orders;

    /// <summary>
    /// This constructor initializes the new ProductOrderDTO with default values.
    /// <para>ProductOrderDTO objects with Id = -1 means that it does not contain any data.</para>
    /// </summary>
    public ProductOrderDTO()
    {
        productOrderId = -1;
        orders = new List<OrderDTO>();
    }
    
    /// <summary>
    /// Provides get and set methods for ProductOrderId property.
    /// </summary>
    /// <value>Property <c>ProductOrderId</c> represents a unique identifier for ProductOrder objects.</value>
    public long ProductOrderId
    {
        get => productOrderId;
        set => productOrderId = value;
    }

    /// <summary>
    /// Provides get and set methods for DeliveryDate property.
    /// </summary>
    /// <value>Property <c>DeliveryDate</c> represents the delivery date for ProductOrder objects.</value>
    public DateTime DeliveryDate
    {
        get => deliveryDate;
        set => deliveryDate = value;
    }

    /// <summary>
    /// Provides get and set methods for OrderingDate property.
    /// </summary>
    /// <value>Property <c>OrderingDate</c> represents the creation date for ProductOrder objects.</value>
    public DateTime OrderingDate
    {
        get => orderingDate;
        set => orderingDate = value;
    }

    /// <summary>
    /// Provides get and set methods for Orders property.
    /// </summary>
    /// <value>Property <c>Orders</c> represents a list of products that contain the ProductOrder objects.</value>
    /// <exception cref="ArgumentNullException">If set Orders with null.</exception>
    public List<OrderDTO> Orders
    {
        get => orders;
        set => orders = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Reports a ProductOrderDTO properties as a string.
    /// </summary>
    /// <returns>A string with the product order properties productOrderId, orderingDate and deliveryDate.</returns>
    public override string ToString()
    {
        return $"Id: {productOrderId}, orderDate: {orderingDate}, deliverDate: {deliveryDate}";
    }
    
    /// <summary>
    /// This method determines whether two ProductOrderDTO have the same properties.
    /// </summary>
    /// <param name="obj">Is the object to be compared to the current object.</param>
    /// <returns>True if ProductOrderDTO are equals otherwise false.</returns>
    public override bool Equals(object? obj)
    {
        var item = obj as ProductOrderDTO;
        
        if (item == null)
        {
            return false;
        }

        return productOrderId.Equals(item.productOrderId) &&
               deliveryDate.Equals(item.deliveryDate) &&
               orderingDate.Equals(item.orderingDate) &&
               orders.SequenceEqual(item.orders);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(productOrderId, orderingDate, deliveryDate);
    }
}
