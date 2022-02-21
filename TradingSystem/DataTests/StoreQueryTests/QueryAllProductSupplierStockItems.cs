using Data.Exceptions;
using Data.Store;
using Xunit;

namespace DataTests.StoreQueryTests;

[Collection("DataTestCollection")]
public class QueryAllProductSupplierStockItems
{
    private readonly DatabaseFixture _fixture;
    private readonly IStoreQuery _storeQuery;

    public QueryAllProductSupplierStockItems(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _storeQuery = new StoreQuery();
    }
    
    [Fact]
    public void Found_All_Product_StockItems()
    {
        const long storeId = 1;
        var result = _storeQuery.QueryAllProductSupplierStockItems(storeId, _fixture.Context);
        Assert.Equal(5, result.Count);
        Assert.Collection(result,
            item => Assert.Equal(1, item.Id), 
            item => Assert.Equal(2, item.Id), 
            item => Assert.Equal(3, item.Id),
            item => Assert.Equal(4, item.Id),
            item => Assert.Equal(5, item.Id)
        );
        Assert.Collection(result,
            item => Assert.Equal(1, item.Product.Id), 
            item => Assert.Equal(2, item.Product.Id), 
            item => Assert.Equal(3, item.Product.Id),
            item => Assert.Equal(4, item.Product.Id),
            item => Assert.Equal(5, item.Product.Id)
        );
        Assert.Contains(result, item => item.Product.ProductSupplier.Name == "Schegel");
        Assert.Contains(result, item => item.Product.ProductSupplier.Name == "Kaufmann");
    }
    
    [Fact]
    public void Found_No_Product_StockItems()
    {
        const long storeId = 23;
        var action = () => _storeQuery.QueryAllProductSupplierStockItems(storeId, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal($"Product stock items from store id '{storeId}' could not be found!", 
            exception.Message);
    }
}