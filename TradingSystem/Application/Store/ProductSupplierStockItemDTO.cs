namespace Application.Store;

/// <summary>
/// Class <c>ProductSupplierStockItemDTO</c> is used as a data transfer object for transferring data from the database.
/// It is used in the Enterprise and Store application to handle the business logic.
/// A DTO can be either a copy of the persisted data for further processing, or
/// to modify or add new data in the persistence layer.
/// </summary>
public class ProductSupplierStockItemDTO : ProductSupplierDTO
{
    protected StockItemDTO stockItem;

    /// <summary>
    /// This constructor initializes the new ProductSupplierStockItemDTO with default values.
    /// <para>ProductSupplierStockItemDTO objects with Id = -1 means that it does not contain any data.</para>
    /// </summary>
    public ProductSupplierStockItemDTO()
    {
        productId = -1;
        barcode = 0;
        purchasePrice = 0;
        productName = "";
        supplierId = -1;
        supplierName = "";
        stockItem = new StockItemDTO();
    }

    /// <summary>
    /// Provides get and set methods for StockItem property.
    /// </summary>
    /// <value>Property <c>StockItem</c> represents a item of the StockItem objects.</value>
    public StockItemDTO StockItem
    {
        get => stockItem;
        set => stockItem = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    /// <summary>
    /// Reports a ProductSupplierStockItemDTO properties as a string.
    /// </summary>
    /// <returns>A string with the all properties.</returns>
    public override string ToString()
    {
        return $"Id: {supplierId}, Supplier: {supplierName}\n" +
               $"\tId: {productId}, Barcode: {barcode}, Product: {productName}, purPrice: {purchasePrice.ToString("F2")} €\n" +
               $"\tId: {stockItem.ItemId}, Amount: {stockItem.Amount}, minStock: {stockItem.MinStock}, " +
               $"maxStock: {stockItem.MaxStock}, salePrice: {stockItem.SalesPrice.ToString("F2")} €\n";
    }

    /// <summary>
    /// This method determines whether two ProductSupplierStockItemDTO have the same properties.
    /// </summary>
    /// <param name="obj">Is the object to be compared to the current object.</param>
    /// <returns>True if ProductSupplierStockItemDTO are equals otherwise false.</returns>
    public override bool Equals(object? obj)
    {
        var item = obj as ProductSupplierStockItemDTO;
        
        if (item == null)
        {
            return false;
        }

        return productId.Equals(item.productId) &&
               barcode.Equals(item.barcode) &&
               purchasePrice.Equals(item.purchasePrice) &&
               productName.Equals(item.productName) &&
               supplierId.Equals(item.supplierId) &&
               supplierName.Equals(item.supplierName) &&
               stockItem.Equals(item.stockItem);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(productId, supplierId, stockItem);
    }
}
