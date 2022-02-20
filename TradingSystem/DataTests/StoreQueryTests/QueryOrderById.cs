using System;
using Data.Exceptions;
using Data.Store;
using Xunit;

namespace DataTests.StoreQueryTests;

[Collection("DataTestCollection")]
public class QueryOrderById
{
    private readonly DatabaseFixture _fixture;
    private readonly IStoreQuery _storeQuery;

    public QueryOrderById(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _storeQuery = new StoreQuery();
    }
    
    [Fact]
    public void Found_Order_By_Id_01()
    {
        const long orderId = 1;
        var result = _storeQuery.QueryOrderById(orderId, _fixture.Context);
        Assert.Equal(orderId, result.Id);
        Assert.Equal(DateTime.Parse("17/02/2022 12:15:45").ToUniversalTime(), result.OrderingDate);
        Assert.Equal(DateTime.Parse("19/02/2022 8:15:32").ToUniversalTime(), result.DeliveryDate);
        Assert.Equal(2,result.OrderEntries.Count);
        Assert.Contains(result.OrderEntries, order => order.Product.ProductSupplier.Name == "Schegel");
        Assert.Collection(result.OrderEntries, 
            order => Assert.Equal(1, order.Product.Id),
            order => Assert.Equal(3, order.Product.Id));
    }
    
    [Fact]
    public void Found_Order_By_Id_02()
    {
        const long orderId = 2;
        var result = _storeQuery.QueryOrderById(orderId, _fixture.Context);
        Assert.Equal(orderId, result.Id);
        Assert.Equal(DateTime.Parse("17/02/2022 12:15:45").ToUniversalTime(), result.OrderingDate);
        Assert.Equal(DateTime.MinValue, result.DeliveryDate);
        Assert.Equal(2,result.OrderEntries.Count);
        Assert.Contains(result.OrderEntries, order => order.Product.ProductSupplier.Name == "Kaufmann");
        Assert.Collection(result.OrderEntries, 
            order => Assert.Equal(4, order.Product.Id),
            order => Assert.Equal(5, order.Product.Id));
    }
    
    [Fact]
    public void Found_No_Order_By_Id()
    {
        const long orderId = 23;
        var action = () => _storeQuery.QueryOrderById(orderId, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal($"Product order with id '{orderId}' could not be found!", exception.Message);
    }
}