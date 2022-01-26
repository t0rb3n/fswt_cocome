using TradingSystem.inventory.data.enterprise;
using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.application.store;
public interface IStoreApplication
{
    public Store GetStore();
    public List<StockItem> GetProductsLowStockItems();
    public List<Product> GetAllProductSuppliers();
    public List<StockItem> GetAllProductsSupplierStockItems();
    public void OrderProducts(ProductOrder productOrder);
    public ProductOrder GetProductOrder(long productOrderId);
    public void RollInReceivedProductOrder(long productOrderId);
    public void ChangePrice(StockItem stockItem);
}