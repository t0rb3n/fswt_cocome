using Data.Exceptions;
using Data.Store;
using Xunit;

namespace DataTests.StoreQueryTests;

[Collection("DataTestCollection")]
public class QueryProductStockItem
{
    private readonly DatabaseFixture _fixture;
    private readonly IStoreQuery _storeQuery;

    public QueryProductStockItem(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _storeQuery = new StoreQuery();
    }
    
    [Fact]
    public void Found_StockItem()
    {
        const long storeId = 1;
        const long barcode = 10000001;
        var result = _storeQuery.QueryProductStockItem(storeId, barcode, _fixture.Context);
        Assert.Equal(2, result.Id);
        Assert.Equal(2, result.Product.Id);
        Assert.Equal(barcode, result.Product.Barcode);
    }
    
    [Fact]
    public void Found_No_StockItem_Barcode()
    {
        const long storeId = 1;
        const long barcode = 10000201;
        var action = () => _storeQuery.QueryProductStockItem(storeId, barcode, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal($"Stock item with barcode '{barcode}' could not be found!", exception.Message);
    }
    
    [Fact]
    public void Found_No_StockItem_Store()
    {
        const long storeId = 23;
        const long barcode = 10000001;
        var action = () => _storeQuery.QueryProductStockItem(storeId, barcode, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal($"Stock item with barcode '{barcode}' could not be found!", exception.Message);
    }
}