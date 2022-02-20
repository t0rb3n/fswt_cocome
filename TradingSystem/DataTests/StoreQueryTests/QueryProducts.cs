using Data.Exceptions;
using Data.Store;
using Xunit;

namespace DataTests.StoreQueryTests;

[Collection("DataTestCollection")]
public class QueryProducts
{
    private readonly DatabaseFixture _fixture;
    private readonly IStoreQuery _storeQuery;

    public QueryProducts(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _storeQuery = new StoreQuery();
    }

    [Fact]
    public void Found_Products()
    {
        const long storeId = 1;
        var result = _storeQuery.QueryProducts(storeId, _fixture.Context);
        Assert.Equal(5, result.Count);
        Assert.Contains(result, product => product.ProductSupplier.Name == "Schegel");
        Assert.Contains(result, product => product.ProductSupplier.Name == "Kaufmann");
    }
    
    [Fact]
    public void Found_No_Products()
    {
        const long storeId = -2;
        var action = () => _storeQuery.QueryProducts(storeId, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal($"Products from store id '{storeId}' could not be found!", exception.Message);
    }
}