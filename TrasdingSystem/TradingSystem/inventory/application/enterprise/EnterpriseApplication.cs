using TradingSystem.inventory.data;
using TradingSystem.inventory.data.enterprise;
using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.application.enterprise;

public class EnterpriseApplication : IEnterpriseApplication
{
    private IStoreQuery storeQuery = IDataFactory.getInstance().getIStoreQuery();
    private IEnterpriseQuery enterpriseQuery = IDataFactory.getInstance().getIEnterpriseQuery();
    
    public Report getStockReport(Store store)
    {
        throw new NotImplementedException();
    }

    public Report getStockReport(Enterprise enterprise)
    {
        throw new NotImplementedException();
    }

    public Report getMeanTimeToDeliveryReport(Enterprise enterprise)
    {
        throw new NotImplementedException();
    }
}