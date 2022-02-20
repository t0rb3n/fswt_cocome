using Data.Exceptions;
using Data.Store;
using Xunit;

namespace DataTests.StoreQueryTests;

[Collection("DataTestCollection")]
public class QueryLowStockItems
{
    private readonly DatabaseFixture _fixture;
    private readonly IStoreQuery _storeQuery;

    public QueryLowStockItems(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _storeQuery = new StoreQuery();
    }
    
    [Fact]
    public void Found_Low_StockItems()
    {
        const long storeId = 1;
        var result = _storeQuery.QueryLowStockItems(storeId, _fixture.Context);
        Assert.Equal(3, result.Count);
        Assert.Collection(result,
            item => Assert.Equal(2, item.Id), 
            item => Assert.Equal(3, item.Id), 
            item => Assert.Equal(5, item.Id)
        );
        Assert.Collection(result,
            item => Assert.Equal(2, item.Product.Id), 
            item => Assert.Equal(3, item.Product.Id), 
            item => Assert.Equal(5, item.Product.Id)
        );
        Assert.Contains(result, item => item.Amount < item.MinStock);
    }
    
    [Fact]
    public void Found_No_Low_StockItems()
    {
        const long storeId = 3;
        var action = () => _storeQuery.QueryLowStockItems(storeId, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal($"Stock items from store id '{storeId}' could not be found!", exception.Message);
    }
}