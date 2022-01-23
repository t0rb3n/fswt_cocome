using TradingSystem.inventory.data;
using TradingSystem.inventory.data.enterprise;
using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.application.store;

public class StoreApplication : IStoreApplication, ICashDeskConnector
{
    private IStoreQuery _storeOuery = IDataFactory.GetInstance().GetStoreQuery();
    private long _storeId;

    public StoreApplication(long storeId)
    {
        this._storeId = storeId;
    }

    public Store GetStore()
    {
        throw new NotImplementedException();
    }

    public List<ProductStockItem> GetProductsWithLowStock()
    {
        throw new NotImplementedException();
    }

    public List<ProductSupplier> GetAllProducts()
    {
        throw new NotImplementedException();
    }

    public List<ProductSupplierStockItem> GetAllProductsSupplierStockItems()
    {
        throw new NotImplementedException();
    }

    public void OrderProducts(ProductOrder productOrder)
    {
        throw new NotImplementedException();
    }

    public ProductOrder GetProductOrder(long productOrderId)
    {
        throw new NotImplementedException();
    }

    public void RollInReceivedProductOrder(long productOrderId)
    {
        throw new NotImplementedException();
    }

    public ProductStockItem ChangePrice(StockItem stockItem)
    {
        throw new NotImplementedException();
    }

    public void BookSale(Sale sale)
    {
        throw new NotImplementedException();
    }

    public ProductStockItem GetProductStockItem(long productBarcode)
    {
        throw new NotImplementedException();
    }
}