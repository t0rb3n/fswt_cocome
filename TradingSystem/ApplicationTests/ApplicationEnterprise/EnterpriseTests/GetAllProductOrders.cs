using System;
using Application.Exceptions;
using Xunit;

namespace ApplicationTests.ApplicationEnterprise.EnterpriseTests;

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
        const long storeId = 2;
        var result = _fixture.EnterpriseApplication.GetAllProductOrders(storeId);
        Assert.Equal(3, result.Count);
        
        // ProductOrder with Id 4
        Assert.Equal(4, result[0].ProductOrderId);
        Assert.Equal(DateTime.Parse("2022/02/13 08:35:31").ToLocalTime(), result[0].OrderingDate);
        Assert.Equal(DateTime.Parse("2022/02/25 06:12:01").ToLocalTime(), result[0].DeliveryDate);
        Assert.Equal(10,result[0].Orders.Count);

        // ProductOrder with Id 5
        Assert.Equal(5, result[1].ProductOrderId);
        Assert.Equal(DateTime.Parse("2022/02/17 06:24:29").ToLocalTime(), result[1].OrderingDate);
        Assert.Equal(DateTime.MinValue, result[1].DeliveryDate);
        Assert.Equal(7,result[1].Orders.Count);
        
        // ProductOrder with Id 6
        Assert.Equal(6, result[2].ProductOrderId);
        Assert.Equal(DateTime.Parse("2022/02/18 12:54:19").ToLocalTime(), result[2].OrderingDate);
        Assert.Equal(DateTime.MinValue, result[2].DeliveryDate);
        Assert.Single(result[2].Orders);
    }

    [Fact]
    public void Failure_Get_All_Product_Order()
    {
        const long storeId = 42;
        var action = () => _fixture.EnterpriseApplication.GetAllProductOrders(storeId);
        var exception = Assert.Throws<EnterpriseException>(action);
        Assert.Equal("Product orders could not be found!", exception.Message);
    }
}