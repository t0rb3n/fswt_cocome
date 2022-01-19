using TradingSystem.inventory.data;
using TradingSystem.inventory.data.enterprise;
using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.application.store;

public class StoreApplication : IStoreApplication, ICashDeskConnector
{
    private IStoreQuery storeOuery = IDataFactory.getInstance().getIStoreQuery();
    private long storeId;

    public StoreApplication(long storeId)
    {
        this.storeId = storeId;
    }

    public Store getStore()
    {
        throw new NotImplementedException();
    }

    public List<ProductStockItem> getProductsWithLowStock()
    {
        throw new NotImplementedException();
    }

    public List<ProductSupplier> getAllProducts()
    {
        throw new NotImplementedException();
    }

    public List<ProductSupplierStockItem> GetAllProductsSupplierStockItems()
    {
        throw new NotImplementedException();
    }

    public void orderProducts(ProductOrder productOrder)
    {
        throw new NotImplementedException();
    }

    public ProductOrder getProductOrder(long productOrderId)
    {
        throw new NotImplementedException();
    }

    public void rollInReceivedProductOrder(long productOrderId)
    {
        throw new NotImplementedException();
    }

    public ProductStockItem changePrice(StockItem stockItem)
    {
        throw new NotImplementedException();
    }

    public void bookSale(Sale sale)
    {
        throw new NotImplementedException();
    }

    public ProductStockItem getProductStockItem(long productBarcode)
    {
        throw new NotImplementedException();
    }
}