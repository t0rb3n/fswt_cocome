using TradingSystem.inventory.data.enterprise;
using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.application.store;

public interface IStoreApplication
{
    public Store GetStore();
    public List<ProductStockItem> GetProductsWithLowStock();
    public List<ProductSupplier> GetAllProducts();
    public List<ProductSupplierStockItem> GetAllProductsSupplierStockItems();
    public void OrderProducts(ProductOrder productOrder);
    public ProductOrder GetProductOrder(long productOrderId);
    public void RollInReceivedProductOrder(long productOrderId);
    public ProductStockItem ChangePrice(StockItem stockItem);
}