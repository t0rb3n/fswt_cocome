namespace Application.Store;

/// <summary>
/// Class <c>OrderDTO</c> is used as a data transfer object for transferring data from the database.
/// It is used in the Enterprise and Store application to handle the business logic.
/// A DTO can be either a copy of the persisted data for further processing, or
/// to modify or add new data in the persistence layer.
/// </summary>
public class OrderDTO
{
    protected long orderId;
    protected int amount;
    protected ProductSupplierDTO productSupplier;

    /// <summary>
    /// This constructor initializes the new OrderDTO with default values.
    /// <para>OrderDTO objects with Id = -1 means that it does not contain any data.</para>
    /// </summary>
    public OrderDTO()
    {
        orderId = -1;
        amount = 0;
        productSupplier = new ProductSupplierDTO();
    }
    
    /// <summary>
    /// Provides get and set methods for OrderId property.
    /// </summary>
    /// <value>Property <c>OrderId</c> represents a unique identifier for OrderEntry objects.</value>
    public long OrderId
    {
        get => orderId;
        set => orderId = value;
    }
    
    /// <summary>
    /// Provides get and set methods for Amount property.
    /// </summary>
    /// <value>Property <c>Amount</c> represents the amount of ordered products for OrderEntry objects.</value>
    public int Amount
    {
        get => amount;
        set => amount = value;
    }

    /// <summary>
    /// Provides get and set methods for ProductSupplier property.
    /// </summary>
    /// <value>Property <c>ProductSupplier</c> represents the supplier of the order.</value>
    /// <exception cref="ArgumentNullException">If set ProductSupplier with null.</exception>
    public ProductSupplierDTO ProductSupplier
    {
        get => productSupplier;
        set => productSupplier = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Reports a OrderDTO properties as a string.
    /// </summary>
    /// <returns>A string with the order properties orderId, amount and productSupplier.</returns>
    public override string ToString()
    {
        return $"Id: {orderId}, amount: {amount}, \n\tproductSupplier: {productSupplier}";
    }
    
    /// <summary>
    /// This method determines whether two OrderDTO have the same properties.
    /// </summary>
    /// <param name="obj">Is the object to be compared to the current object.</param>
    /// <returns>True if OrderDTO are equals otherwise false.</returns>
    public override bool Equals(object? obj)
    {
        var item = obj as OrderDTO;
        
        if (item == null)
        {
            return false;
        }

        return orderId.Equals(item.orderId) &&
               amount.Equals(item.amount) &&
               productSupplier.Equals(item.productSupplier);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(orderId, amount);
    }
}
