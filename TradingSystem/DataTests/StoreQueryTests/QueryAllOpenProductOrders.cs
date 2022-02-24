using System;
using Data.Exceptions;
using Data.Store;
using Xunit;

namespace DataTests.StoreQueryTests;

[Collection("DataTestCollection")]
public class QueryAllOpenProductOrders
{
    private readonly DatabaseFixture _fixture;
    private readonly IStoreQuery _storeQuery;

    public QueryAllOpenProductOrders(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _storeQuery = new StoreQuery();
    }
    
    [Fact]
    public void Found_All_Open_Orders()
    {
        const long storeId = 2;
        var result = _storeQuery.QueryAllOpenProductOrders(storeId, _fixture.Context);
        Assert.Equal(2, result.Count);
        
        // ProductOrder with Id 5
        Assert.Equal(5, result[0].Id);
        Assert.Equal(DateTime.Parse("2022/02/17 06:24:29").ToLocalTime(), result[0].OrderingDate);
        Assert.Equal(DateTime.MinValue, result[0].DeliveryDate);
        Assert.Equal(7,result[0].OrderEntries.Count);
        Assert.Contains(result[0].OrderEntries, order => order.Product.ProductSupplier.Name == "Scheffler GmbH");
        
        // ProductOrder with Id 1
        Assert.Equal(6, result[1].Id);
        Assert.Equal(DateTime.Parse("2022/02/18 12:54:19").ToLocalTime(), result[1].OrderingDate);
        Assert.Equal(DateTime.MinValue, result[1].DeliveryDate);
        Assert.Single(result[1].OrderEntries);
        Assert.Contains(result[1].OrderEntries, order => order.Product.ProductSupplier.Name == "Betz Lange");
    }
    
    [Fact]
    public void Found_No_Open_Orders()
    {
        const long storeId = 5;
        var action = () => _storeQuery.QueryAllOpenProductOrders(storeId, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal($"Open product orders from store id '{storeId}' could not be found!", exception.Message);
    }
}