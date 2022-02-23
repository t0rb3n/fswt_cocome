using System;
using Application.Exceptions;
using Xunit;

namespace ApplicationTests.ApplicationStore.StoreTests;

[Collection("ApplicationTestCollection")]
public class GetAllProductOrders
{
    private readonly DatabaseFixture _fixture;

    public GetAllProductOrders(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Success_Get_All_Product_Order()
    {
        var result = _fixture.StoreApplication.GetAllProductOrders();
        Assert.Equal(3, result.Count);

        // ProductOrder with Id 1
        Assert.Equal(1, result[0].ProductOrderId);
        Assert.Equal(DateTime.Parse("2022/02/17 10:16:53").ToLocalTime(), result[0].OrderingDate);
        Assert.Equal(DateTime.MinValue, result[0].DeliveryDate);
        Assert.Equal(3, result[0].Orders.Count);

        // ProductOrder with Id 2
        Assert.Equal(2, result[1].ProductOrderId);
        Assert.Equal(DateTime.Parse("2022/02/02 20:35:24").ToLocalTime(), result[1].OrderingDate);
        Assert.Equal(DateTime.Parse("2022/02/25 05:29:59").ToLocalTime(), result[1].DeliveryDate);
        Assert.Equal(12, result[1].Orders.Count);

        // ProductOrder with Id 3
        Assert.Equal(3, result[2].ProductOrderId);
        Assert.Equal(DateTime.Parse("2022/02/16 10:47:30").ToLocalTime(), result[2].OrderingDate);
        Assert.Equal(DateTime.Parse("2022/02/27 15:53:33").ToLocalTime(), result[2].DeliveryDate);
        Assert.Equal(3, result[2].Orders.Count);
    }

    [Fact]
    public void Failure_Get_All_Product_Order()
    {
        var action = () => _fixture.StoreApplicationFailure.GetAllProductOrders();
        var exception = Assert.Throws<StoreException>(action);
        Assert.Equal("One or more errors occurred. (Product orders could not be found!)", 
            exception.Message);
    }
}