using Data.Enterprise;
using Data.Exceptions;
using Xunit;

namespace DataTests.EnterpriseQueryTests;

[Collection("DataTestCollection")]
public class QueryEnterpriseById
{
    private readonly DatabaseFixture _fixture;
    private readonly IEnterpriseQuery _enterpriseQuery;

    public QueryEnterpriseById(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _enterpriseQuery = new EnterpriseQuery();
    }

    [Fact]
    public void Found_Enterprise_By_Id()
    {
        const long enterpriseId = 1;
        var result = _enterpriseQuery.QueryEnterpriseById(enterpriseId, _fixture.Context);
        Assert.Equal(1, result.Id);
        Assert.Equal("CocomeSystem GmbH & Co. KG", result.Name);
    }
    
    [Fact]
    public void Found_No_Enterprise_By_Id()
    {
        const long enterpriseId = -2;
        var action = () => _enterpriseQuery.QueryEnterpriseById(enterpriseId, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal($"Enterprise with the id '{enterpriseId}' could not be found!", exception.Message);
    }
}