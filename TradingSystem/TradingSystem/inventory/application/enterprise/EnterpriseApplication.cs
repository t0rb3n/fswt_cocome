using TradingSystem.inventory.data;
using TradingSystem.inventory.data.enterprise;
using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.application.enterprise;

public class EnterpriseApplication : IEnterpriseApplication
{
    private IStoreQuery _storeQuery = IDataFactory.GetInstance().GetStoreQuery();
    private IEnterpriseQuery _enterpriseQuery = IDataFactory.GetInstance().GetEnterpriseQuery();

    public Report GetStockReport(Store store)
    {
        throw new NotImplementedException();
    }

    public Report GetStockReport(Enterprise enterprise)
    {
        throw new NotImplementedException();
    }

    public Report GetMeanTimeToDeliveryReport(Enterprise enterprise)
    {
        throw new NotImplementedException();
    }
}