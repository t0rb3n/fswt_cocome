namespace Application.Store;
public interface IStoreApplication
{
    public StoreEnterpriseDTO GetStore();
    public IList<ProductStockItemDTO> GetProductsLowStockItems();
    public IList<ProductSupplierDTO> GetAllProductSuppliers();
    public IList<ProductSupplierStockItemDTO> GetAllProductSupplierStockItems();
    public void OrderProducts(ProductOrderDTO productOrder);
    public ProductOrderDTO GetProductOrder(long productOrderId);
    public void RollInReceivedProductOrder(long productOrderId);
    public void ChangePrice(long stockItemId, double newPrice);
}