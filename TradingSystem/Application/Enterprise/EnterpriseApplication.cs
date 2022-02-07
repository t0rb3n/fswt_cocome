using Application.Store;
using Data;
using Data.Enterprise;
using Data.Store;

namespace Application.Enterprise;

public class EnterpriseApplication : IEnterpriseApplication
{
    private readonly IStoreQuery _storeQuery = IDataFactory.GetInstance().GetStoreQuery();
    private readonly IEnterpriseQuery _enterpriseQuery = IDataFactory.GetInstance().GetEnterpriseQuery();
    private readonly long _enterpriseId;

    public EnterpriseApplication(long enterpriseId)
    {
        _enterpriseId = enterpriseId;
    }

    public EnterpriseDTO GetEnterprise()
    {
        EnterpriseDTO result = new();

        using var dbc = new DatabaseContext();
        using var ctx = dbc.Database.BeginTransaction();
        
        try
        {
            var query = _enterpriseQuery.QueryEnterpriseById(_enterpriseId, dbc);
            result = ConvertEntryObject.ToEnterpriseDTO(query);
            ctx.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return result;
    }
    
    public bool ChangePrice(StockItem stockItem)
    {
        using var dbc = new DatabaseContext();
        using var ctx = dbc.Database.BeginTransaction();
        try
        {
            var result = _storeQuery.QueryStockItemById(stockItem.Id, dbc);
            result.SalesPrice = stockItem.SalesPrice;
            dbc.SaveChanges();
            ctx.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return true;
    }

    public ReportDTO GetStockReport(Data.Store.Store store)
    {
        throw new NotImplementedException();
    }

    public ReportDTO GetStockReport(Data.Enterprise.Enterprise enterprise)
    {
        throw new NotImplementedException();
    }

    public ReportDTO GetMeanTimeToDeliveryReport(Data.Enterprise.Enterprise enterprise)
    {
        throw new NotImplementedException();
    }
}