using TradingSystem.inventory.data.enterprise;
using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.application.store;
public interface IStoreApplication
{
    public StoreEnterpriseDTO GetStore();
    public List<ProductStockItemDTO> GetProductsLowStockItems();
    public List<ProductSupplierDTO> GetAllProductSuppliers();
    public List<ProductSupplierStockItemDTO> GetAllProductSupplierStockItems();
    public void OrderProducts(ProductOrderDTO productOrderDto);
    public ProductOrderDTO GetProductOrder(long productOrderId);
    public void RollInReceivedProductOrder(long productOrderId);
    public void ChangePrice(StockItemDTO stockItem);
}