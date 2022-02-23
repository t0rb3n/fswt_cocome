using System;
using System.Linq;
using Application.Exceptions;
using Application.Store;
using Xunit;

namespace ApplicationTests.ApplicationEnterprise.EnterpriseTests;

[Collection("ApplicationTestCollection")]
public class RollInReceivedProductOrder
{
    private readonly DatabaseFixture _fixture;

    public RollInReceivedProductOrder(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Success_Received_Product_Order()
    {
        const long productOrderId = 1;
        const long storeId = 1;
        var receivedProductOrder = _fixture.EnterpriseApplication.GetProductOrder(productOrderId);
        receivedProductOrder.DeliveryDate = DateTime.Now;
        _fixture.EnterpriseApplication.RollInReceivedProductOrder(receivedProductOrder, storeId);
        // Converts nanosecond to microsecond 
        receivedProductOrder.DeliveryDate = new DateTime(receivedProductOrder.DeliveryDate.Ticks / 10 * 10);
        var result = _fixture.EnterpriseApplication.GetProductOrder(productOrderId);
        Assert.Equal(receivedProductOrder, result);
    }
    
    [Fact]
    public void Failure_Received_Product_Order_01()
    {
        const long storeId = 1;
        var productOrder = new ProductOrderDTO();
        var action = () => _fixture.EnterpriseApplication.RollInReceivedProductOrder(productOrder, storeId);
        var exception = Assert.Throws<EnterpriseException>(action);
        Assert.Equal("Product order could not be found!", exception.Message);
    }
    
    [Fact]
    public void Failure_Received_Product_Order_02()
    {
        const long productOrderId = 6;
        const long storeId = 42;
        var receivedProductOrder = _fixture.EnterpriseApplication.GetProductOrder(productOrderId);
        receivedProductOrder.DeliveryDate = DateTime.Now;
        var action = () => _fixture.EnterpriseApplication.RollInReceivedProductOrder(receivedProductOrder, storeId);
        var exception = Assert.Throws<EnterpriseException>(action);
        Assert.Equal("Product order can not be executed from this store!", exception.Message);
    }
    
    [Fact]
    public void Failure_Received_Product_Order_03()
    {
        const long productOrderId = 1;
        const long storeId = 1;
        var receivedProductOrder = _fixture.EnterpriseApplication.GetProductOrder(productOrderId);
        var action = () => _fixture.EnterpriseApplication.RollInReceivedProductOrder(receivedProductOrder, storeId);
        var exception = Assert.Throws<EnterpriseException>(action);
        Assert.Equal("Product order has already been received!", exception.Message);
    }
}