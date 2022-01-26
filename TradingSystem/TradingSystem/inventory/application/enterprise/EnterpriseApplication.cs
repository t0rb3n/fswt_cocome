using TradingSystem.inventory.data;
using TradingSystem.inventory.data.enterprise;
using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.application.enterprise;

public class EnterpriseApplication : IEnterpriseApplication
{
    private IStoreQuery _storeQuery = IDataFactory.GetInstance().GetStoreQuery();
    private IEnterpriseQuery _enterpriseQuery = IDataFactory.GetInstance().GetEnterpriseQuery();

    public ReportDTO GetStockReport(Store store)
    {
        throw new NotImplementedException();
    }

    public ReportDTO GetStockReport(Enterprise enterprise)
    {
        throw new NotImplementedException();
    }

    public ReportDTO GetMeanTimeToDeliveryReport(Enterprise enterprise)
    {
        throw new NotImplementedException();
    }
}