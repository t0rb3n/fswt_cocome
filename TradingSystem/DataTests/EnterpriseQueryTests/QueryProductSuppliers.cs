using Data.Enterprise;
using Data.Exceptions;
using Xunit;

namespace DataTests.EnterpriseQueryTests;

[Collection("DataTestCollection")]
public class QueryProductSuppliers
{
    private readonly DatabaseFixture _fixture;
    private readonly IEnterpriseQuery _enterpriseQuery;

    public QueryProductSuppliers(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _enterpriseQuery = new EnterpriseQuery();
    }

    [Fact]
    public void Found_All_Product_Suppliers()
    {
        const long enterpriseId = 1;
        var result = _enterpriseQuery.QueryProductSuppliers(enterpriseId, _fixture.Context);
        Assert.Equal(6,result.Count);
    }
    
    [Fact]
    public void Found_No_Product_Suppliers()
    {
        const long enterpriseId = -2;
        var action = () => _enterpriseQuery.QueryProductSuppliers(enterpriseId, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal($"Product suppliers from enterprise id '{enterpriseId}' could not be found!", 
            exception.Message);
    }
}