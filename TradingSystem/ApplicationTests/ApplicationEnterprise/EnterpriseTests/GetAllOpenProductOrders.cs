using System;
using Application.Exceptions;
using Xunit;

namespace ApplicationTests.ApplicationEnterprise.EnterpriseTests;

[Collection("ApplicationTestCollection")]
public class GetAllOpenProductOrders
{
    private readonly DatabaseFixture _fixture;

    public GetAllOpenProductOrders(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public void Success_Get_All_Open_Product_Order()
    {
        const long storeId = 3;
        var result = _fixture.EnterpriseApplication.GetAllOpenProductOrders(storeId);
        Assert.Equal(1, result.Count);
        
        // ProductOrder with Id 8
        Assert.Equal(8, result[0].ProductOrderId);
        Assert.Equal(DateTime.Parse("2022/02/10 16:51:28").ToLocalTime(), result[0].OrderingDate);
        Assert.Equal(DateTime.MinValue, result[0].DeliveryDate);
        Assert.Contains(result[0].Orders, order => order.ProductSupplier.SupplierName == "Lutz GmbH");
        Assert.Single(result[0].Orders);
    }

    [Fact]
    public void Failure_Get_All_Open_Product_Order()
    {
        const long storeId = 42;
        var action = () => _fixture.EnterpriseApplication.GetAllOpenProductOrders(storeId);
        var exception = Assert.Throws<EnterpriseException>(action);
        Assert.Equal("Open product orders could not be found!", exception.Message);
    }
}