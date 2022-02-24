using Application.Exceptions;

namespace Application.Store;

/// <summary>
/// The interface IStoreApplication provides methods for the web server application component.
/// </summary>
public interface IStoreApplication
{
    /// <summary>
    /// Gets information of the store in which the component is running.
    /// This information is retrieved by the component during configuration and initialization.
    /// </summary>
    /// <returns>Store and enterprise information about the local store.</returns>
    /// <exception cref="StoreException">If the rpc call failed.</exception>
    public StoreEnterpriseDTO GetStore();

    /// <summary>
    /// Determines products and stock items that are nearly out of stock, meaning amount is lower than minimum amount.
    /// </summary>
    /// <returns>A list of products and their stock item.</returns>
    /// <exception cref="StoreException">If the rpc call failed.</exception>
    public IList<ProductSupplierStockItemDTO> GetLowProductSupplierStockItems();
    
    /// <summary>
    /// Determines all products of the portfolio of a given store and the supplier for each of them.
    /// </summary>
    /// <returns>A list of products and their suppliers</returns>
    /// <exception cref="StoreException">If the rpc call failed.</exception>
    public IList<ProductSupplierDTO> GetAllProductSuppliers();
    
    /// <summary>
    /// Determines all products of the portfolio of a given store and the supplier for each of them.
    /// Additionally the corresponding stock items are queried
    /// </summary>
    /// <returns>A list of products, their suppliers and the corresponding stock item if they have any</returns>
    /// <exception cref="StoreException">If the rpc call failed.</exception>
    public IList<ProductSupplierStockItemDTO> GetAllProductSupplierStockItems();
    
    /// <summary>
    /// Creates a list of orders for different suppliers for an initial list of products to be ordered by a store.
    /// ProductOrderDTO is persisted and ordering date is set to date of method execution.
    /// </summary>
    /// <param name="productOrder">The product order dto with the list of orders dto.</param>
    /// <exception cref="StoreException">If the rpc call failed.</exception>
    public void OrderProducts(ProductOrderDTO productOrder);
    
    /// <summary>
    /// Returns order information for a given order id.
    /// </summary>
    /// <param name="productOrderId">The id of the order.</param>
    /// <returns>A detailed order information of the desired order.</returns>
    /// <exception cref="StoreException">If the rpc call failed.</exception>
    public ProductOrderDTO GetProductOrder(long productOrderId);
    
    /// <summary>
    /// Returns all product orders that this owns store.
    /// </summary>
    /// <returns>A list of product orders with their order entries, supplier and product.</returns>
    /// <exception cref="StoreException">If the rpc call failed.</exception>
    public IList<ProductOrderDTO> GetAllProductOrders();
    
    /// <summary>
    /// Returns all open product orders that this owns store.
    /// </summary>
    /// <returns>A list of open product orders with their order entries, supplier and product.</returns>
    /// <exception cref="StoreException">If the rpc call failed.</exception>
    public IList<ProductOrderDTO> GetAllOpenProductOrders();
    
    /// <summary>
    /// Updates stocks after order delivery. Adds amount of ordered items to the stock items of the store.
    /// Sets delivery date to date of method execution.
    /// </summary>
    /// <param name="productOrderId">The id of the order.</param>
    /// <exception cref="StoreException">If the rpc call failed.</exception>
    public void RollInReceivedProductOrder(long productOrderId);
    
    /// <summary>
    /// Updates sales price of a stock item.
    /// </summary>
    /// <param name="stockItemId">The id of the stock item.</param>
    /// <param name="newPrice">The new price of the stock item.</param>
    /// <exception cref="StoreException">If the rpc call failed.</exception>
    public void ChangePrice(long stockItemId, double newPrice);
}
