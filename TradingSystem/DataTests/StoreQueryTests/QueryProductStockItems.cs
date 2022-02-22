using System;
using Data.Exceptions;
using Data.Store;
using Xunit;

namespace DataTests.StoreQueryTests;

[Collection("DataTestCollection")]
public class QueryProductStockItems
{
    private readonly DatabaseFixture _fixture;
    private readonly IStoreQuery _storeQuery;

    public QueryProductStockItems(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _storeQuery = new StoreQuery();
    }
    
    [Fact]
    public void Found_StockItems()
    {
        const long storeId = 1;
        long[] productIds = { 77, 111, 140 };
        var result = _storeQuery.QueryProductStockItems(storeId, productIds, _fixture.Context);
        Assert.Equal(3, result.Count);
        Assert.Collection(result,
            item => Assert.Equal(77, item.Product.Id), 
            item => Assert.Equal(111, item.Product.Id), 
            item => Assert.Equal(140, item.Product.Id)
            );
    }
    
    [Fact]
    public void Found_No_StockItems_Product_List()
    {
        const long storeId = 1;
        long[] productIds = { 1,-2,3 };
        var action = () => _storeQuery.QueryProductStockItems(storeId, productIds, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal("One or more stock items in the product list could not be found!",
            exception.Message);
    }
    
    [Fact]
    public void Found_No_StockItems_Empty_Product_List()
    {
        const long storeId = 23;
        long[] productIds = Array.Empty<long>();
        var action = () => _storeQuery.QueryProductStockItems(storeId, productIds, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal("Stock items could not be found because the productIds array was empty!", 
            exception.Message);
    }
    
    [Fact]
    public void Found_No_StockItems_Store()
    {
        const long storeId = 2;
        long[] productIds = { 1,-2,3 };
        var action = () => _storeQuery.QueryProductStockItems(storeId, productIds, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal("One or more stock items in the product list could not be found!",
            exception.Message);
    }
}