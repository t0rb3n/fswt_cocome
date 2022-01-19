using TradingSystem.inventory.data.enterprise;
using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.application.store;

public interface IStoreApplication
{
    public Store getStore();
    public List<ProductStockItem> getProductsWithLowStock();
    public List<ProductSupplier> getAllProducts();
    public List<ProductSupplierStockItem> GetAllProductsSupplierStockItems();
    public void orderProducts(ProductOrder productOrder);
    public ProductOrder getProductOrder(long productOrderId);
    public void rollInReceivedProductOrder(long productOrderId);
    public ProductStockItem changePrice(StockItem stockItem);
}