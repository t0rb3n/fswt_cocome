using Application.Store;

namespace Application.Enterprise;

/// <summary>
/// Class <c>StoreStockReportDTO</c> is used as a data transfer object for transferring data from the database.
/// It is used in the Enterprise and Store application to handle the business logic.
/// A DTO can be either a copy of the persisted data for further processing, or
/// to modify or add new data in the persistence layer.
/// </summary>
public class StoreStockReportDTO : StoreDTO
{
    protected List<ProductSupplierStockItemDTO> stockItem;

    /// <summary>
    /// This constructor initializes the new StoreStockReportDTO with default values.
    /// <para>StoreStockReportDTO objects with Id = -1 means that it does not contain any data.</para>
    /// </summary>
    public StoreStockReportDTO()
    {
        storeId = -1;
        storeName = "";
        location = "";
        stockItem = new List<ProductSupplierStockItemDTO>();
    }

    /// <summary>
    /// Provides get and set methods for StockItems property.
    /// </summary>
    /// <value>Property <c>StockItems</c> represents a list of <see cref="ProductSupplierStockItemDTO"/>.</value>
    /// <exception cref="ArgumentNullException">If set StockItems with null.</exception>
    public List<ProductSupplierStockItemDTO> StockItems
    {
        get => stockItem;
        set => stockItem = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Reports a StoreStockReportDTO properties as a string.
    /// </summary>
    /// <returns>A string with storeId, storeName, properties of this store and a list of stock items.</returns>
    public override string ToString()
    {
        var printList = "";
        foreach (var item in StockItems)
        {
            printList += $"\t{item}\n";
        }
        return $"Id: {storeId}, Store: {storeName}, Location: {location}\n{printList}";
    }
    
    /// <summary>
    /// This method determines whether two StoreStockReportDTO have the same properties.
    /// </summary>
    /// <param name="obj">Is the object to be compared to the current object.</param>
    /// <returns>True if StoreStockReportDTO are equals otherwise false.</returns>
    public override bool Equals(object? obj)
    {
        var item = obj as StoreStockReportDTO;

        if (item == null)
        {
            return false;
        }

        return storeId.Equals(item.storeId) &&
               storeName.Equals(item.storeName) &&
               location.Equals(item.location) &&
               stockItem.Equals(item.stockItem);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(storeId, location, stockItem);
    }
}