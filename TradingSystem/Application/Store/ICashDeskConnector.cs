namespace Application.Store;

/// <summary>
/// The interface ICashDeskConnector provides methods for the web server application component.
/// </summary>
public interface ICashDeskConnector
{
    /// <summary>
    /// Registers the selling of products contained in the stock of the store. Updates amount of stock items.
    /// </summary>
    /// <param name="saleDto">The SaleDTO to adjust the stock in the warehouse.</param>
    public void BookSale(SaleDTO saleDto);
    
    /// <summary>
    /// Determines product and the item in the stock of the store by the given barcode.
    /// </summary>
    /// <param name="productBarcode">Contains the given barcode</param>
    /// <returns>
    /// A <see cref="ProductStockItemDTO"/> instance which contains the id product
    /// which is linked to the stock item of the store.
    /// </returns>
    public ProductStockItemDTO GetProductStockItem(long productBarcode);
}
