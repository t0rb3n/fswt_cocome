namespace Data.Enterprise;

public class EnterpriseQuery : IEnterpriseQuery
{
    public Enterprise QueryEnterpriseById(long enterpriseId, DatabaseContext dbc)
    {
        var result = dbc
                         .Enterprises
                         .Find(enterpriseId)
                     ?? throw new Exception($"Can't find enterprise by id {enterpriseId}");
        return result;
    }

    public long QueryMeanTimeToDelivery(ProductSupplier productSupplier, Enterprise enterprise, DatabaseContext dbc)
    {
        throw new NotImplementedException();
    }
}