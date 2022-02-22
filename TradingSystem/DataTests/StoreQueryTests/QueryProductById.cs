using Data.Exceptions;
using Data.Store;
using Xunit;

namespace DataTests.StoreQueryTests;

[Collection("DataTestCollection")]
public class QueryProductById
{
    private readonly DatabaseFixture _fixture;
    private readonly IStoreQuery _storeQuery;

    public QueryProductById(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _storeQuery = new StoreQuery();
    }
    
    [Fact]
    public void Found_Product_By_Id()
    {
        const long productId = 1;
        var result = _storeQuery.QueryProductById(productId, _fixture.Context);
        Assert.Equal(productId, result.Id);
        Assert.Equal(10000000, result.Barcode);
        Assert.Equal("Funny-frisch Chipsfrisch", result.Name);
        Assert.Equal(23.15, result.PurchasePrice);
    }
    
    [Fact]
    public void Found_No_Product_By_Id()
    {
        const long productId = -23;
        var action = () => _storeQuery.QueryProductById(productId, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal($"Product with id '{productId}' could not be found!", exception.Message);
    }
}