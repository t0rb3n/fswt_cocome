using System;
using Application.Exceptions;
using Application.Store;
using Xunit;

namespace ApplicationTests.ApplicationStore.StoreTests;

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
        const long productOrderId = 11;
        var receivedProductOrder = _fixture.StoreApplication.GetProductOrder(productOrderId);
        receivedProductOrder.DeliveryDate = DateTime.Now;
        
        _fixture.StoreApplication.RollInReceivedProductOrder(receivedProductOrder.ProductOrderId);
        
        var result = _fixture.StoreApplication.GetProductOrder(productOrderId);
        Assert.NotEqual(DateTime.MinValue, result.DeliveryDate);
        Assert.True(receivedProductOrder.DeliveryDate.Ticks < result.DeliveryDate.Ticks);
        
        // After the test set the order and delivery date to a fixed date for the GetMeanTimeToDeliveryReport tests
        var po = _fixture.Context.ProductOrders.Find(productOrderId)!;
        var fixDate = DateTime.Parse("2022/02/21 07:29:30");
        po.OrderingDate = fixDate.ToUniversalTime();
        po.DeliveryDate = fixDate.AddHours(5).ToUniversalTime(); 
        _fixture.Context.SaveChanges();
    }
    
    [Fact]
    public void Failure_Received_Product_Order_01()
    {
        var productOrder = new ProductOrderDTO();
        var action = () => _fixture.StoreApplication.RollInReceivedProductOrder(productOrder.ProductOrderId);
        var exception = Assert.Throws<StoreException>(action);
        Assert.Equal("Product order could not be found!", exception.Message);
    }
    
    [Fact]
    public void Failure_Received_Product_Order_02()
    {
        const long productOrderId = 6;
        var receivedProductOrder = _fixture.StoreApplication.GetProductOrder(productOrderId);
        var action = () => 
            _fixture.StoreApplicationFailure.RollInReceivedProductOrder(receivedProductOrder.ProductOrderId);
        var exception = Assert.Throws<StoreException>(action);
        Assert.Equal("Product order can not be executed from this store!", exception.Message);
    }
    
    [Fact]
    public void Failure_Received_Product_Order_03()
    {
        const long productOrderId = 2;
        var receivedProductOrder = _fixture.StoreApplication.GetProductOrder(productOrderId);
        var action = () => _fixture.StoreApplication.RollInReceivedProductOrder(receivedProductOrder.ProductOrderId);
        var exception = Assert.Throws<StoreException>(action);
        Assert.Equal("Product order has already been received!", exception.Message);
    }
}