using System;
using Data.Exceptions;
using Data.Store;
using Xunit;

namespace DataTests.StoreQueryTests;

[Collection("DataTestCollection")]
public class QueryAllProductOrders
{
    private readonly DatabaseFixture _fixture;
    private readonly IStoreQuery _storeQuery;

    public QueryAllProductOrders(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _storeQuery = new StoreQuery();
    }
    
    [Fact]
    public void Found_All_Orders()
    {
        const long storeId = 1;
        var result = _storeQuery.QueryAllProductOrders(storeId, _fixture.Context);
        Assert.Equal(3, result.Count);
        
        // ProductOrder with Id 1
        Assert.Equal(1, result[0].Id);
        Assert.Equal(DateTime.Parse("2022/02/17 10:16:53").ToLocalTime(), result[0].OrderingDate);
        Assert.Equal(DateTime.MinValue, result[0].DeliveryDate);
        Assert.Equal(12,result[0].OrderEntries.Count);
        Assert.Contains(result[0].OrderEntries, order => order.Product.ProductSupplier.Name == "Lutz GmbH");

        // ProductOrder with Id 2
        Assert.Equal(2, result[1].Id);
        Assert.Equal(DateTime.Parse("2022/02/02 20:35:24").ToLocalTime(), result[1].OrderingDate);
        Assert.Equal(DateTime.Parse("2022/02/25 05:29:59").ToLocalTime(), result[1].DeliveryDate);
        Assert.Equal(9,result[1].OrderEntries.Count);
        Assert.Contains(result[1].OrderEntries, order => order.Product.ProductSupplier.Name == "Scheffler GmbH");
        
        // ProductOrder with Id 3
        Assert.Equal(3, result[2].Id);
        Assert.Equal(DateTime.Parse("2022/02/16 10:47:30").ToLocalTime(), result[2].OrderingDate);
        Assert.Equal(DateTime.Parse("2022/02/27 15:53:33").ToLocalTime(), result[2].DeliveryDate);
        Assert.Equal(6,result[2].OrderEntries.Count);
        Assert.Contains(result[2].OrderEntries, order => order.Product.ProductSupplier.Name == "Betz Lange");
    }
    
    [Fact]
    public void Found_No_Orders()
    {
        const long storeId = 5;
        var action = () => _storeQuery.QueryAllProductOrders(storeId, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal($"Product orders from store id '{storeId}' could not be found!", exception.Message);
    }
}