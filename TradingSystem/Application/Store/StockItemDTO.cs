namespace Application.Store;

/// <summary>
/// Class <c>StockItemDTO</c> is used as a data transfer object for transferring data from the database.
/// It is used in the Enterprise and Store application to handle the business logic.
/// A DTO can be either a copy of the persisted data for further processing, or
/// to modify or add new data in the persistence layer.
/// </summary>
public class StockItemDTO
{
    protected long itemId;
    protected double salesPrice;
    protected int amount;
    protected int minStock;
    protected int maxStock;

    /// <summary>
    /// This constructor initializes the new StockItemDTO with default values.
    /// <para>StockItemDTO objects with Id = -1 means that it does not contain any data.</para>
    /// </summary>
    public StockItemDTO()
    {
        itemId = -1;
        salesPrice = 0;
        amount = 0;
        minStock = 0;
        maxStock = 0;
    }

    /// <summary>
    /// Provides get and set methods for ItemId property.
    /// </summary>
    /// <value>Property <c>ItemId</c> represents a unique identifier for StockItem objects.</value>
    public long ItemId
    {
        get => itemId;
        set => itemId = value;
    }

    /// <summary>
    /// Provides get and set SalesPrice for ItemId property.
    /// </summary>
    /// <value>Property <c>SalesPrice</c> represents the sales price for StockItem objects.</value>
    public double SalesPrice
    {
        get => salesPrice;
        set => salesPrice = value;
    }

    /// <summary>
    /// Provides get and set Amount for ItemId property.
    /// </summary>
    /// <value>Property <c>Amount</c> represents the amount of this item from StockItem objects.</value>
    public int Amount
    {
        get => amount;
        set => amount = value;
    }

    /// <summary>
    /// Provides get and set MinStock for ItemId property.
    /// </summary>
    /// <value>Property <c>MinStock</c> represents the minimum amount of this item from StockItem objects.</value>
    public int MinStock
    {
        get => minStock;
        set => minStock = value;
    }

    /// <summary>
    /// Provides get and set MaxStock for ItemId property.
    /// </summary>
    /// <value>Property <c>MaxStock</c> represents the maximum amount of this item from StockItem objects.</value>
    public int MaxStock
    {
        get => maxStock;
        set => maxStock = value;
    }

    /// <summary>
    /// Reports a StockItemDTO properties as a string.
    /// </summary>
    /// <returns>A string with the stock item properties itemId, amount, minStock, maxStock and salesPrice.</returns>
    public override string ToString()
    {
        return $"Id: {itemId}, Amount: {amount}, minStock: {minStock}, " +
               $"maxStock: {maxStock}, salePrice: {salesPrice.ToString("F2")} €\n";
    }
}
