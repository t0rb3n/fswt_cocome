using TradingSystem.inventory.data.enterprise;
using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.application.enterprise;

public interface IEnterpriseApplication
{
    public Report getStockReport(Store store);
    public Report getStockReport(Enterprise enterprise);
    public Report getMeanTimeToDeliveryReport(Enterprise enterprise);
}