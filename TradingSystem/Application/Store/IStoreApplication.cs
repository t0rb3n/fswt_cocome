namespace Application.Store;
public interface IStoreApplication
{
    public StoreEnterpriseDTO GetStore();
    public List<ProductStockItemDTO> GetProductsLowStockItems();
    public List<ProductSupplierDTO> GetAllProductSuppliers();
    public List<ProductSupplierStockItemDTO> GetAllProductSupplierStockItems();
    public void OrderProducts(ProductOrderDTO productOrder);
    public ProductOrderDTO GetProductOrder(long productOrderId);
    public void RollInReceivedProductOrder(long productOrderId);
    public void ChangePrice(long stockItemId, double newPrice);
}