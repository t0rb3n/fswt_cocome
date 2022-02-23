using System;
using System.Collections.Generic;
using System.Linq;
using Application.Exceptions;
using Application.Store;
using Xunit;

namespace ApplicationTests.ApplicationEnterprise.EnterpriseTests;

[Collection("ApplicationTestCollection")]
public class OrderProducts
{
    private readonly DatabaseFixture _fixture;

    public OrderProducts(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Success_Order_Products()
    {
        const long storeId = 1;
        var productList = _fixture.EnterpriseApplication.GetAllProductSuppliers(1);
        var lastProductOrder = _fixture.Context.ProductOrders.OrderBy(order => order.Id).LastOrDefault()!;
        var lastOrderEntry = _fixture.Context.OrderEntries.OrderBy(order => order.Id).LastOrDefault()!;
        
        var orderList = new List<OrderDTO>()
        {
            new OrderDTO
            {
                OrderId = lastOrderEntry.Id + 1,
                Amount = 4,
                ProductSupplier = productList.Single(item => item.ProductId == 140)
            },
            new OrderDTO
            {
                OrderId = lastOrderEntry.Id + 2,
                Amount = 4,
                ProductSupplier = productList.Single(item => item.ProductId == 135)
            },
            new OrderDTO
            {
                OrderId = lastOrderEntry.Id + 3,
                Amount = 4,
                ProductSupplier = productList.Single(item => item.ProductId == 136)
            }
        };
       
        ProductOrderDTO productOrder = new()
        {
            ProductOrderId = lastProductOrder.Id + 1,
            OrderingDate = DateTime.Now,
            DeliveryDate = DateTime.MinValue
        };
        productOrder.Orders = orderList;
        _fixture.EnterpriseApplication.OrderProducts(productOrder, storeId);
        // Converts nanosecond to microsecond 
        productOrder.OrderingDate = new DateTime(productOrder.OrderingDate.Ticks / 10 * 10);
        var testProductOrderDto = _fixture.EnterpriseApplication.GetProductOrder(productOrder.ProductOrderId);
        Assert.Equal(productOrder, testProductOrderDto);
    }

    [Fact]
    public void Failure_Order_Products_01()
    {
        const long storeId = 1;
        var productOrder = new ProductOrderDTO();
        var action = () => _fixture.EnterpriseApplication.OrderProducts(productOrder, storeId);
        var exception = Assert.Throws<EnterpriseException>(action);
        Assert.Equal("Product order contains no order entries!", exception.Message);
    }
    
    [Fact]
    public void Failure_Order_Products_02()
    {
        const long storeId = 1;
        var productList = _fixture.EnterpriseApplication.GetAllProductSuppliers(1);

        var orderList = new List<OrderDTO>()
        {
            new OrderDTO
            {
                OrderId = 0,
                Amount = 4,
                ProductSupplier = productList.Single(item => item.ProductId == 140)
            },
            new OrderDTO
            {
                OrderId = 0,
                Amount = 4,
                ProductSupplier = productList.Single(item => item.ProductId == 135)
            },
            new OrderDTO
            {
                OrderId = 0,
                Amount = 4,
                ProductSupplier = productList.Single(item => item.ProductId == 136)
            }
        };
        
        ProductOrderDTO productOrder = new()
        {
            ProductOrderId = 0,
            DeliveryDate = DateTime.MinValue
        };
        productOrder.Orders = orderList;
        var action = () => _fixture.EnterpriseApplication.OrderProducts(productOrder, storeId);
        var exception = Assert.Throws<EnterpriseException>(action);
        Assert.Equal("Order date was not set!", exception.Message);
    }
    
    [Fact]
    public void Failure_Order_Products_03()
    {
        const long storeId = -12;
        var productList = _fixture.EnterpriseApplication.GetAllProductSuppliers(1);

        var orderList = new List<OrderDTO>()
        {
            new OrderDTO
            {
                OrderId = 0,
                Amount = 4,
                ProductSupplier = productList.Single(item => item.ProductId == 140)
            },
            new OrderDTO
            {
                OrderId = 0,
                Amount = 4,
                ProductSupplier = productList.Single(item => item.ProductId == 135)
            },
            new OrderDTO
            {
                OrderId = 0,
                Amount = 4,
                ProductSupplier = productList.Single(item => item.ProductId == 136)
            }
        };
        
        ProductOrderDTO productOrder = new()
        {
            ProductOrderId = 0,
            OrderingDate = DateTime.UtcNow,
            DeliveryDate = DateTime.MinValue
        };
        productOrder.Orders = orderList;
        var action = () => _fixture.EnterpriseApplication.OrderProducts(productOrder, storeId);
        var exception = Assert.Throws<EnterpriseException>(action);
        Assert.Equal("An unexpected error occurred while executing the order!", exception.Message);
    }
}