using System;
using Application.Exceptions;
using Xunit;

namespace ApplicationTests.ApplicationStore.StoreTests;

[Collection("ApplicationTestCollection")]
public class GetProductOrder
{
    private readonly DatabaseFixture _fixture;

    public GetProductOrder(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Success_Get_Product_Order_01()
    {
        const long productOrderId = 1;
        var result = _fixture.StoreApplication.GetProductOrder(productOrderId);
        Assert.Equal(productOrderId, result.ProductOrderId);
        Assert.Equal(DateTime.Parse("2022/02/17 10:16:53").ToLocalTime(), result.OrderingDate);
        Assert.Equal(DateTime.MinValue, result.DeliveryDate);
        Assert.Equal(3, result.Orders.Count);
    }
    
    [Fact]
    public void Success_Get_Product_Order_02()
    {
        const long productOrderId = 3;
        var result = _fixture.StoreApplication.GetProductOrder(productOrderId);
        Assert.Equal(productOrderId, result.ProductOrderId);
        Assert.Equal(DateTime.Parse("2022/02/16 10:47:30").ToLocalTime(), result.OrderingDate);
        Assert.Equal(DateTime.Parse("2022/02/27 15:53:33").ToLocalTime(), result.DeliveryDate);
        Assert.Equal(3, result.Orders.Count);
    }
    
    [Fact]
    public void Failure_Get_Product_Order()
    {
        const long productOrderId = 42;
        var action = () => _fixture.StoreApplication.GetProductOrder(productOrderId);
        var exception = Assert.Throws<StoreException>(action);
        Assert.Equal("Product order could not be found!", exception.Message);
    }
}