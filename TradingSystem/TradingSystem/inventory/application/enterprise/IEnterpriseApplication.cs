using TradingSystem.inventory.data.enterprise;
using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.application.enterprise;

public interface IEnterpriseApplication
{
    public Report GetStockReport(Store store);
    public Report GetStockReport(Enterprise enterprise);
    public Report GetMeanTimeToDeliveryReport(Enterprise enterprise);
}