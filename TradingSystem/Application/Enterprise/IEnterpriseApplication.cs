using Data.Store;

namespace Application.Enterprise;

public interface IEnterpriseApplication
{
    public EnterpriseDTO GetEnterprise();
    public ReportDTO GetStockReport(Data.Store.Store store);
    public ReportDTO GetStockReport(Data.Enterprise.Enterprise enterprise);
    public ReportDTO GetMeanTimeToDeliveryReport(Data.Enterprise.Enterprise enterprise);
    public bool ChangePrice(StockItem stockItem);
}