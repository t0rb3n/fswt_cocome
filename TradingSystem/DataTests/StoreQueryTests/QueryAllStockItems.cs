using Data.Exceptions;
using Data.Store;
using Xunit;

namespace DataTests.StoreQueryTests;

[Collection("DataTestCollection")]
public class QueryAllStockItems
{
    private readonly DatabaseFixture _fixture;
    private readonly IStoreQuery _storeQuery;

    public QueryAllStockItems(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _storeQuery = new StoreQuery();
    }
    
    [Fact]
    public void Found_All_StockItems()
    {
        var result = _storeQuery.QueryAllStockItems(1, _fixture.Context);
        Assert.Equal(5, result.Count);
        Assert.Collection(result,
            item => Assert.Equal(1, item.Id), 
            item => Assert.Equal(2, item.Id), 
            item => Assert.Equal(3, item.Id),
            item => Assert.Equal(4, item.Id),
            item => Assert.Equal(5, item.Id)
        );
    }
    
    [Fact]
    public void Found_No_StockItems()
    {
        var action = () => _storeQuery.QueryLowStockItems(3, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal("Stock items from store id '3' could not be found!", exception.Message);
    }
}