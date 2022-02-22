using Data.Exceptions;
using Data.Store;
using Xunit;

namespace DataTests.StoreQueryTests;

[Collection("DataTestCollection")]
public class QueryAllProductStockItems
{
    private readonly DatabaseFixture _fixture;
    private readonly IStoreQuery _storeQuery;

    public QueryAllProductStockItems(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _storeQuery = new StoreQuery();
    }
    
    [Fact]
    public void Found_All_StockItems()
    {
        const long storeId = 1;
        
        var result = _storeQuery.QueryAllProductStockItems(storeId, _fixture.Context);
        Assert.Equal(63, result.Count);
    }
    
    [Fact]
    public void Found_No_StockItems()
    {
        const long storeId = 4;
        var action = () => _storeQuery.QueryLowProductSupplierStockItems(storeId, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal($"Stock items from store id '{storeId}' could not be found!", exception.Message);
    }
}