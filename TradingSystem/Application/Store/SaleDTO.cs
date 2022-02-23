namespace Application.Store;

/// <summary>
/// Class <c>SaleDTO</c> is used as a data transfer object for transferring data from the database.
/// It is used in the Enterprise and Store application to handle the business logic.
/// A DTO can be either a copy of the persisted data for further processing, or
/// to modify or add new data in the persistence layer.
/// </summary>
public class SaleDTO
{
    protected DateTime date;
    protected List<ProductStockItemDTO> products;

    /// <summary>
    /// This constructor initializes the new SaleDTO with a new list of <see cref="ProductStockItemDTO"/>.
    /// </summary>
    public SaleDTO()
    {
        products = new List<ProductStockItemDTO>();
    }

    /// <summary>
    /// Provides get and set methods for Date property.
    /// </summary>
    /// <value>Property <c>Date</c> represents the date of the sale.</value>
    public DateTime Date
    {
        get => date;
        set => date = value;
    }

    /// <summary>
    /// Provides get and set methods for Products property.
    /// </summary>
    /// <value>Property <c>Products</c> represents a list of products sold at this sale.</value>
    public List<ProductStockItemDTO> Products
    {
        get => products;
        set => products = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    /// <summary>
    /// Reports a SaleDTO properties as a string.
    /// </summary>
    /// <returns>A string with the date of the sale and the number of products sold.</returns>
    public override string ToString()
    {
        return $"Sales date: {date}, Number of products: {products.Count}";
    }
    
    /// <summary>
    /// This method determines whether two SaleDTO have the same properties.
    /// </summary>
    /// <param name="obj">Is the object to be compared to the current object.</param>
    /// <returns>True if SaleDTO are equals otherwise false.</returns>
    public override bool Equals(object? obj)
    {
        var item = obj as SaleDTO;
        
        if (item == null)
        {
            return false;
        }

        return date.Equals(item.date) &&
               products.Equals(item.products);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(date, products);
    }
}
