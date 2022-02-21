using Data.Enterprise;
using Data.Exceptions;

namespace Data.Store;

/// <summary>
/// Interface IStoreQuery provides methods for querying the database.
/// </summary>
public interface IStoreQuery
{
    /// <summary>
    /// Queries for a specified store and adds the <see cref="Enterprise"/> reference.
    /// </summary>
    /// <param name="storeId">A unique id of the store.</param>
    /// <param name="dbc">A database context to query.</param>
    /// <returns>A <see cref="Store"/> which has this id.</returns>
    /// <exception cref="ItemNotFoundException">If no store was found.</exception>
    public Store QueryStoreById(long storeId, DatabaseContext dbc);
    
    /// <summary>
    /// Queries for the products that owned this store and adds the <see cref="ProductSupplier"/> reference.
    /// </summary>
    /// <param name="storeId">A unique id of the store.</param>
    /// <param name="dbc">A database context to query.</param>
    /// <returns>A list of <see cref="Product"/> owned by this store.</returns>
    /// <exception cref="ItemNotFoundException">If no products were found by this store.</exception>
    public IList<Product> QueryProductSuppliers(long storeId, DatabaseContext dbc);
    
    /// <summary>
    /// Queries for a specified product.
    /// </summary>
    /// <param name="productId">A unique id of the product.</param>
    /// <param name="dbc">A database context to query.</param>
    /// <returns>A <see cref="Product"/> which has this id.</returns>
    /// <exception cref="ItemNotFoundException">If product was not found.</exception>
    public Product QueryProductById(long productId, DatabaseContext dbc);
    
    /// <summary>
    /// Queries stock items that are running out of stock at this store and
    /// adds the <see cref="Product"/> and <see cref="ProductSupplier"/> reference.
    /// </summary>
    /// <param name="storeId">A unique id of the store.</param>
    /// <param name="dbc">A database context to query.</param>
    /// <returns>A list of <see cref="StockItem"/> that are low on this store.</returns>
    /// <exception cref="ItemNotFoundException">If no stock items were found by this store.</exception>
    public IList<StockItem> QueryLowProductSupplierStockItems(long storeId, DatabaseContext dbc);
    
    /// <summary>
    /// Queries all stock items that owned this store and adds the <see cref="Product"/> reference.
    /// </summary>
    /// <param name="storeId">A unique id of the store</param>
    /// <param name="dbc">A database context to query</param>
    /// <returns>A list of <see cref="StockItem"/> by this store.</returns>
    /// <exception cref="ItemNotFoundException">If no stock items were found by this store</exception>
    public IList<StockItem> QueryAllProductStockItems(long storeId, DatabaseContext dbc);
    
    /// <summary>
    /// Queries for all stock items that own this store and adds the <see cref="Product"/>
    /// and <see cref="ProductSupplier"/> reference.
    /// </summary>
    /// <param name="storeId">A unique id of the store.</param>
    /// <param name="dbc">A database context to query.</param>
    /// <returns>A list of <see cref="Product"/> owned by this store.</returns>
    /// <exception cref="ItemNotFoundException">If no products were found by this store.</exception>
    public IList<StockItem> QueryAllProductSupplierStockItems(long storeId, DatabaseContext dbc);
    
    /// <summary>
    /// Queries for a product order which owned this store and adds the
    /// <see cref="OrderEntry"/>, <see cref="Product"/> and <see cref="ProductSupplier"/> reference.
    /// </summary>
    /// <param name="orderId">A unique id of the ProductOrder.</param>
    /// <param name="dbc">A database context to query.</param>
    /// <returns>A <see cref="ProductOrder"/> which has this id.</returns>
    /// <exception cref="ItemNotFoundException">If no product order was found.</exception>
    public ProductOrder QueryProductOrderById(long orderId, DatabaseContext dbc);
    
    /// <summary>
    /// Queries for all product order which owned this store and adds the
    /// <see cref="OrderEntry"/>, <see cref="Product"/> and <see cref="ProductSupplier"/> reference.
    /// </summary>
    /// <param name="storeId">A unique id of the store.</param>
    /// <param name="dbc">A database context to query.</param>
    /// <returns>A list of <see cref="ProductOrder"/> owned by this store.</returns>
    /// <exception cref="ItemNotFoundException">If no product orders were found.</exception>
    public IList<ProductOrder> QueryAllProductOrders(long storeId, DatabaseContext dbc);
    
    /// <summary>
    /// Queries for a stock item with the barcode of the product from this store and
    /// adds the <see cref="Product"/> reference.
    /// </summary>
    /// <param name="storeId">A unique id of the store.</param>
    /// <param name="barcode">The barcode of the product.</param>
    /// <param name="dbc">A database context to query.</param>
    /// <returns>A <see cref="StockItem"/> with the corresponding barcode.</returns>
    /// <exception cref="ItemNotFoundException">If no stock item with the product barcode
    /// was found by this store.</exception>
    public StockItem QueryProductStockItem(long storeId, long barcode, DatabaseContext dbc);
    
    /// <summary>
    /// Queries for stock items with a product id array of products from this store and
    /// adds the <see cref="Product"/> reference.
    /// </summary>
    /// <param name="storeId">A unique id of the store.</param>
    /// <param name="productIds">The products to look up in the stock.</param>
    /// <param name="dbc">A database context to query.</param>
    /// <returns>A list of <see cref="StockItem"/> based on the product ids at this store.</returns>
    /// <exception cref="ItemNotFoundException">If no stock item with the product ids
    /// was found by this store.</exception>
    public IList<StockItem> QueryProductStockItems(long storeId, long[] productIds, DatabaseContext dbc);
    
    /// <summary>
    /// Queries for a stock item and adds the <see cref="Product"/> reference.
    /// </summary>
    /// <param name="stockId">A unique id of the store.</param>
    /// <param name="dbc">A database context to query.</param>
    /// <returns>A <see cref="StockItem"/> which has this id.</returns>
    /// <exception cref="ItemNotFoundException">If no stock item was found.</exception>
    public StockItem QueryProductStockItemById(long stockId, DatabaseContext dbc); 
}