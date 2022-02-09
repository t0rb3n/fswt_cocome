using Application.Store;

namespace Application.Enterprise;

public interface IEnterpriseApplication
{
    public EnterpriseDTO GetEnterprise();
    public ReportDTO GetStockReport(Data.Store.Store store);
    public ReportDTO GetStockReport(Data.Enterprise.Enterprise enterprise);
    public ReportDTO GetMeanTimeToDeliveryReport(Data.Enterprise.Enterprise enterprise);
    public StoreEnterpriseDTO GetStore(long storeId);
    public List<ProductStockItemDTO> GetProductsLowStockItems(long storeId);
    public List<ProductSupplierDTO> GetAllProductSuppliers(long storeId);
    public List<ProductSupplierStockItemDTO> GetAllProductSupplierStockItems(long storeId);
    public void OrderProducts(ProductOrderDTO productOrder, long storeId);
    public ProductOrderDTO GetProductOrder(long productOrderId);
    public void RollInReceivedProductOrder(ProductOrderDTO productOrder, long storeId);
    public bool ChangePrice(long stockItemId, double newPrice);
    public void MakeBookSale(SaleDTO saleDto);
    public ProductStockItemDTO GetProductStockItem(long productBarcode, long storeId);
    
}