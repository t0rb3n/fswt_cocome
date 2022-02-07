using Data.Store;

namespace Enterprise.Application;

public interface IEnterpriseApplication
{
    public EnterpriseDTO GetEnterprise();
    public ReportDTO GetStockReport(Data.Store.Store store);
    public ReportDTO GetStockReport(Data.Enterprise.Enterprise enterprise);
    public ReportDTO GetMeanTimeToDeliveryReport(Data.Enterprise.Enterprise enterprise);
    public bool ChangePrice(long stockItemId, double newPrice);
}