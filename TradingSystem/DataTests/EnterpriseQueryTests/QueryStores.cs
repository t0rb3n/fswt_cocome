using Data.Enterprise;
using Data.Exceptions;
using Xunit;

namespace DataTests.EnterpriseQueryTests;

[Collection("DataTestCollection")]
public class QueryStores
{
    private readonly DatabaseFixture _fixture;
    private readonly IEnterpriseQuery _enterpriseQuery;

    public QueryStores(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _enterpriseQuery = new EnterpriseQuery();
    }

    [Fact]
    public void Found_All_Stores()
    {
        const long enterpriseId = 1;
        var result = _enterpriseQuery.QueryStores(enterpriseId, _fixture.Context);
        Assert.Equal(2,result.Count);
        Assert.Collection(result,
            store => Assert.Equal(1, store.Id),
            store => Assert.Equal(2, store.Id) 
        );
    }
    
    [Fact]
    public void Found_No_Stores()
    {
        const long enterpriseId = -2;
        var action = () => _enterpriseQuery.QueryStores(enterpriseId, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal($"Stores from enterprise id '{enterpriseId}' could not be found!", 
            exception.Message);
    }
}