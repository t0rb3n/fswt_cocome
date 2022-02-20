using System;
using Data.Exceptions;
using Data.Store;
using Xunit;

namespace DataTests.StoreQueryTests;

[Collection("DataTestCollection")]
public class QueryAllOrders
{
    private readonly DatabaseFixture _fixture;
    private readonly IStoreQuery _storeQuery;

    public QueryAllOrders(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _storeQuery = new StoreQuery();
    }
    
    [Fact]
    public void Found_All_Orders()
    {
        const long storeId = 1;
        var result = _storeQuery.QueryAllOrders(storeId, _fixture.Context);
        Assert.Equal(2, result.Count);
        
        // ProductOrder with Id 1
        Assert.Equal(1, result[0].Id);
        Assert.Equal(DateTime.Parse("17/02/2022 12:15:45").ToUniversalTime(), result[0].OrderingDate);
        Assert.Equal(DateTime.Parse("19/02/2022 8:15:32").ToUniversalTime(), result[0].DeliveryDate);
        Assert.Equal(2,result[0].OrderEntries.Count);
        Assert.Contains(result[0].OrderEntries, order => order.Product.ProductSupplier.Name == "Schegel");
        Assert.Collection(result[0].OrderEntries, 
            order => Assert.Equal(1, order.Product.Id),
            order => Assert.Equal(3, order.Product.Id));
        
        // ProductOrder with Id 2
        Assert.Equal(2, result[1].Id);
        Assert.Equal(DateTime.Parse("17/02/2022 12:15:45").ToUniversalTime(), result[1].OrderingDate);
        Assert.Equal(DateTime.MinValue, result[1].DeliveryDate);
        Assert.Equal(2,result[1].OrderEntries.Count);
        Assert.Contains(result[1].OrderEntries, order => order.Product.ProductSupplier.Name == "Kaufmann");
        Assert.Collection(result[1].OrderEntries, 
            order => Assert.Equal(4, order.Product.Id),
            order => Assert.Equal(5, order.Product.Id));
    }
    
    [Fact]
    public void Found_No_Orders()
    {
        const long storeId = 3;
        var action = () => _storeQuery.QueryAllOrders(storeId, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal($"Product orders from store id '{storeId}' could not be found!", exception.Message);
    }
}