using Data.Exceptions;
using Data.Store;
using Xunit;

namespace DataTests.StoreQueryTests;

[Collection("DataTestCollection")]
public class QueryProductStockItemById
{
    private readonly DatabaseFixture _fixture;
    private readonly IStoreQuery _storeQuery;

    public QueryProductStockItemById(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _storeQuery = new StoreQuery();
    }
    
    [Fact]
    public void Found_StockItem_By_Id()
    {
        const long stockItemId = 1;
        var result = _storeQuery.QueryProductStockItemById(stockItemId, _fixture.Context);
        Assert.Equal(1, result.Id);
        Assert.Equal(39.19, result.SalesPrice);
        Assert.Equal(77, result.Product.Id);
        Assert.Equal(27.99, result.Product.PurchasePrice);
        Assert.Equal("Hot Dog", result.Product.Name);
    }
    
    [Fact]
    public void Found_No_StockItem_By_Id()
    {
        const long stockItemId = 0;
        var action = () => _storeQuery.QueryProductStockItemById(stockItemId, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal($"Stock item with id '{stockItemId}' could not be found!", exception.Message);
    }
}