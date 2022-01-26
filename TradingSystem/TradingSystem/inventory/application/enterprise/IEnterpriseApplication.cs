using TradingSystem.inventory.data.enterprise;
using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.application.enterprise;

public interface IEnterpriseApplication
{
    public ReportDTO GetStockReport(Store store);
    public ReportDTO GetStockReport(Enterprise enterprise);
    public ReportDTO GetMeanTimeToDeliveryReport(Enterprise enterprise);
}