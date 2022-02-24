using System;
using Application.Exceptions;
using Xunit;

namespace ApplicationTests.ApplicationStore.StoreTests;

[Collection("ApplicationTestCollection")]
public class QueryAllOpenProductOrders
{
    private readonly DatabaseFixture _fixture;

    public QueryAllOpenProductOrders(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Success_Get_All_Open_Product_Order()
    {
        var result = _fixture.StoreApplication.GetAllOpenProductOrders();
        Assert.Equal(4, result.Count);

        // ProductOrder with Id 1
        Assert.Equal(1, result[0].ProductOrderId);
        Assert.Equal(DateTime.Parse("2022/02/17 10:16:53").ToLocalTime(), result[0].OrderingDate);
        Assert.Equal(DateTime.MinValue, result[0].DeliveryDate);
        Assert.Contains(result[0].Orders, order => order.ProductSupplier.SupplierName == "Lutz GmbH");
        Assert.Equal(3, result[0].Orders.Count);
    }

    [Fact]
    public void Failure_Get_All_Open_Product_Order()
    {
        var action = () => _fixture.StoreApplicationFailure.GetAllOpenProductOrders();
        var exception = Assert.Throws<StoreException>(action);
        Assert.Equal("One or more errors occurred. (Open product orders could not be found!)", 
            exception.Message);
    }
}