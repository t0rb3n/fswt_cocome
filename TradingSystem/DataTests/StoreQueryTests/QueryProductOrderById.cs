using System;
using Data.Exceptions;
using Data.Store;
using Xunit;

namespace DataTests.StoreQueryTests;

[Collection("DataTestCollection")]
public class QueryProductOrderById
{
    private readonly DatabaseFixture _fixture;
    private readonly IStoreQuery _storeQuery;

    public QueryProductOrderById(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _storeQuery = new StoreQuery();
    }
    
    [Fact]
    public void Found_Order_By_Id_01()
    {
        const long orderId = 1;
        var result = _storeQuery.QueryProductOrderById(orderId, _fixture.Context);
        Assert.Equal(orderId, result.Id);
        Assert.Equal(DateTime.Parse("2022/02/17 10:16:53").ToLocalTime(), result.OrderingDate);
        Assert.Equal(DateTime.MinValue, result.DeliveryDate);
        Assert.Equal(12,result.OrderEntries.Count);
        Assert.Contains(result.OrderEntries, order => order.Product.ProductSupplier.Name == "Lutz GmbH");
    }
    
    [Fact]
    public void Found_Order_By_Id_02()
    {
        const long orderId = 2;
        var result = _storeQuery.QueryProductOrderById(orderId, _fixture.Context);
        Assert.Equal(orderId, result.Id);
        Assert.Equal(DateTime.Parse("2022/02/02 20:35:24").ToLocalTime(), result.OrderingDate);
        Assert.Equal(DateTime.Parse("2022/02/25 05:29:59-00").ToLocalTime(), result.DeliveryDate);
        Assert.Equal(9,result.OrderEntries.Count);
        Assert.Contains(result.OrderEntries, order => order.Product.ProductSupplier.Name == "Scheffler GmbH");
    }
    
    [Fact]
    public void Found_No_Order_By_Id()
    {
        const long orderId = 23;
        var action = () => _storeQuery.QueryProductOrderById(orderId, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal($"Product order with id '{orderId}' could not be found!", exception.Message);
    }
}