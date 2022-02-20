using Data.Exceptions;
using Data.Store;
using Xunit;

namespace DataTests.StoreQueryTests;

[Collection("DataTestCollection")]
public class QueryStoreById
{
    private readonly DatabaseFixture _fixture;
    private readonly IStoreQuery _storeQuery;

    public QueryStoreById(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _storeQuery = new StoreQuery();
    }

    [Fact]
    public void Found_Store_By_Id()
    {
        const long storeId = 1;
        var result = _storeQuery.QueryStoreById(storeId, _fixture.Context);
        Assert.Equal(1, result.Id);
        Assert.Equal("Cocome", result.Name);
        Assert.Equal("Wiesbaden", result.Location);
        Assert.Equal(1, result.Enterprise.Id);
        Assert.Equal("CocomeSystem GmbH & Co. KG", result.Enterprise.Name);
    }
    
    [Fact]
    public void Found_No_Store_By_Id()
    {
        const long storeId = -2;
        var action = () => _storeQuery.QueryStoreById(storeId, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal($"Store with the id '{storeId}' could not be found!", exception.Message);
    }
}
