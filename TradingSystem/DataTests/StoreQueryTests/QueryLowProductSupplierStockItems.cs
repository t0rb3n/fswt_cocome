using Data.Exceptions;
using Data.Store;
using Xunit;

namespace DataTests.StoreQueryTests;

[Collection("DataTestCollection")]
public class QueryLowProductSupplierStockItems
{
    private readonly DatabaseFixture _fixture;
    private readonly IStoreQuery _storeQuery;

    public QueryLowProductSupplierStockItems(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _storeQuery = new StoreQuery();
    }
    
    [Fact]
    public void Found_Low_StockItems()
    {
        const long storeId = 1;
        var result = _storeQuery.QueryLowProductSupplierStockItems(storeId, _fixture.Context);
        Assert.Equal(13, result.Count);
        Assert.Contains(result, item => item.Amount < item.MinStock);
    }
    
    [Fact]
    public void Found_No_Low_StockItems()
    {
        const long storeId = 25;
        var action = () => _storeQuery.QueryLowProductSupplierStockItems(storeId, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal($"Stock items from store id '{storeId}' could not be found!", exception.Message);
    }
}