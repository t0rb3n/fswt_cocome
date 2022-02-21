using Application.Exceptions;
using Application.Store;

namespace Application.Enterprise;

/// <summary>
/// The interface IEnterpriseApplication provides methods for the web enterprise application component.
/// </summary>
public interface IEnterpriseApplication
{
    /// <summary>
    /// Gets information of the enterprise in which the component is running.
    /// This information is retrieved by the component during configuration and initialization.
    /// </summary>
    /// <returns>Information about the enterprise.</returns>
    /// <exception cref="EnterpriseException">If no enterprise was found.</exception>
    public EnterpriseDTO GetEnterprise();
    
    /// <summary>
    /// Gets information about the stores that the enterprise has.
    /// </summary>
    /// <returns>A list of stores in the given enterprise.</returns>
    /// <exception cref="EnterpriseException">If no stores were found for this enterprise.</exception>
    public IList<StoreDTO> GetEnterpriseStores();
    
    /// <summary>
    /// Gets information about the product supplier that the enterprise has.
    /// </summary>
    /// <returns>A list of product supplier in the given enterprise.</returns>
    /// <exception cref="EnterpriseException">If no product supplier were found for this enterprise.</exception>
    public IList<ProductSupplierDTO> GetEnterpriseProductSupplier();
    
    /// <summary>
    /// Gets information of a specified store.
    /// </summary>
    /// <param name="storeId">The id of the specified store.</param>
    /// <returns>Store and enterprise information from this specified store.</returns>
    /// <exception cref="EnterpriseException">If no specified store was found.</exception>
    public StoreEnterpriseDTO GetStore(long storeId);
    
    /// <summary>
    /// Determines low stock items of a specified store.
    /// </summary>
    /// <param name="storeId">The id of the specified store.</param>
    /// <returns>A list of products and their stock item from this specified store.</returns>
    /// <exception cref="EnterpriseException">If no low stock items from the specified store were found.</exception>
    public IList<ProductSupplierStockItemDTO> GetProductsLowStockItems(long storeId);

    /// <summary>
    /// Determines all products of the specified store and the supplier for each of them.
    /// </summary>
    /// <param name="storeId">The id of the specified store.</param>
    /// <returns>A list of products and their suppliers from this specified store.</returns>
    /// <exception cref="EnterpriseException">If no products from the specified store were found.</exception>
    public IList<ProductSupplierDTO> GetAllProductSuppliers(long storeId);
    
    /// <summary>
    /// Determines all products of the specified store and the supplier for each of them and corresponding stock item.
    /// </summary>
    /// <param name="storeId">The id of the store.</param>
    /// <returns>A list of products, their suppliers and the corresponding
    /// stock item from this specified store.</returns>
    /// <exception cref="EnterpriseException">If no low stock items from the specified store were found.</exception>
    public IList<ProductSupplierStockItemDTO> GetAllProductSupplierStockItems(long storeId);
    
    /// <summary>
    /// Takes the order from the store application and adds it to the database.
    /// </summary>
    /// <param name="productOrder">The product order to be made.</param>
    /// <param name="storeId">The id of the specified store.</param>
    /// <exception cref="EnterpriseException">If the product order from the specified store did not work.</exception>
    public void OrderProducts(ProductOrderDTO productOrder, long storeId);
    
    /// <summary>
    /// Gets information about product order for a specified order.
    /// </summary>
    /// <param name="productOrderId">The id of the order.</param>
    /// <returns>A detailed order information of the desired order.</returns>
    /// <exception cref="EnterpriseException">If no product order was found.</exception>
    public ProductOrderDTO GetProductOrder(long productOrderId);
    
    /// <summary>
    /// Gets information about all product orders for a specified store.
    /// </summary>
    /// <param name="storeId">The id of the store.</param>
    /// <returns>A list of product orders from this specified store.</returns>
    /// <exception cref="EnterpriseException">If no product orders were found.</exception>
    public IList<ProductOrderDTO> GetAllProductOrders(long storeId);
    
    /// <summary>
    /// Updates stocks from this specified store after order delivery.
    /// Adds amount of ordered items to the stock items of the specified store.
    /// </summary>
    /// <param name="productOrder">The product order that was delivered.</param>
    /// <param name="storeId">The id of the specified store.</param>
    /// <exception cref="EnterpriseException">
    /// If the product order is not found or the order has already been processed or
    /// does not belong to the specified store.
    /// </exception>
    public void RollInReceivedProductOrder(ProductOrderDTO productOrder, long storeId);
    
    /// <summary>
    /// Updates the sales price of a stock item from the store application in the database.
    /// </summary>
    /// <param name="stockItemId">The id of the stock item.</param>
    /// <param name="newPrice">The new price of the stock item.</param>
    /// <exception cref="EnterpriseException">If no stock item was found.</exception>
    public void ChangePrice(long stockItemId, double newPrice);
    
    /// <summary>
    /// Processes the sale of products from the store application,
    /// which are included in the inventory of the specified store.
    /// Updates amount of stock items from this specified store.
    /// </summary>
    /// <param name="saleDto">The SaleDTO to adjust the stock in the warehouse.</param>
    /// <exception cref="EnterpriseException">If no stock items were found.</exception>
    public void MakeBookSale(SaleDTO saleDto);
    
    /// <summary>
    /// Determines product and the item in the stock of a specified store by the given barcode.
    /// </summary>
    /// <param name="productBarcode">The barcode of the product.</param>
    /// <param name="storeId">The id of the specified store.</param>
    /// <returns></returns>
    /// <exception cref="EnterpriseException">If no product from the specified store was found.</exception>
    public ProductStockItemDTO GetProductStockItem(long productBarcode, long storeId);
    
}
