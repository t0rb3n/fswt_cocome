using System;
using System.Collections.Generic;
using System.Linq;
using Application.Exceptions;
using Application.Store;
using Xunit;

namespace ApplicationTests.ApplicationStore.StoreTests;

[Collection("ApplicationTestCollection")]
public class OrderProducts
{
    private readonly DatabaseFixture _fixture;

    public OrderProducts(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Success_Order_Products_01()
    {
        var productList = _fixture.StoreApplication.GetAllProductSuppliers();
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
            DeliveryDate = DateTime.MinValue,
            Orders = orderList
        };

        _fixture.StoreApplication.OrderProducts(productOrder);
        
        var testProductOrderDto = _fixture.EnterpriseApplication.GetProductOrder(productOrder.ProductOrderId);
        productOrder.OrderingDate = testProductOrderDto.OrderingDate;
        Assert.Equal(productOrder, testProductOrderDto);
    }
    
    [Fact]
    public void Success_Order_Products_02()
    {
        var productList = _fixture.StoreApplication.GetAllProductSuppliers();
        var lastProductOrder = _fixture.Context.ProductOrders.OrderBy(order => order.Id).LastOrDefault()!;
        
        var orderList = new List<OrderDTO>()
        {
            new OrderDTO
            {
                Amount = 4,
                ProductSupplier = productList.Single(item => item.ProductId == 140)
            },
            new OrderDTO
            {
                Amount = 4,
                ProductSupplier = productList.Single(item => item.ProductId == 104)
            },
            new OrderDTO
            {
                Amount = 4,
                ProductSupplier = productList.Single(item => item.ProductId == 135)
            },
            new OrderDTO
            { 
                Amount = 4,
                ProductSupplier = productList.Single(item => item.ProductId == 136)
            },
            new OrderDTO
            {
                Amount = 4,
                ProductSupplier = productList.Single(item => item.ProductId == 122)
            }
        };
       
        ProductOrderDTO productOrder = new() {Orders = orderList};
        
        _fixture.StoreApplication.OrderProducts(productOrder);

        var expectedId01 = lastProductOrder.Id + 1;
        var productOrder01 = _fixture.EnterpriseApplication.GetProductOrder(lastProductOrder.Id + 1);
        Assert.Equal(expectedId01, productOrder01.ProductOrderId);
        Assert.Equal(3, productOrder01.Orders.Count);
        
        var expectedId02 = lastProductOrder.Id + 2;
        var productOrder02 = _fixture.EnterpriseApplication.GetProductOrder(lastProductOrder.Id + 2);
        Assert.Equal(expectedId02, productOrder02.ProductOrderId);
        Assert.Equal(2, productOrder02.Orders.Count);
    }

    [Fact]
    public void Failure_Order_Products_01()
    {
        var productOrder = new ProductOrderDTO();
        var action = () => _fixture.StoreApplication.OrderProducts(productOrder);
        var exception = Assert.Throws<StoreException>(action);
        Assert.Equal("One or more errors occurred. (Product order contains no order entries!)", 
            exception.Message);
    }
    
    [Fact]
    public void Failure_Order_Products_02()
    {
        var productList = _fixture.StoreApplication.GetAllProductSuppliers();

        var orderList = new List<OrderDTO>()
        {
            new OrderDTO
            {
                Amount = 4,
                ProductSupplier = productList.Single(item => item.ProductId == 140)
            },
            new OrderDTO
            {
                Amount = 4,
                ProductSupplier = productList.Single(item => item.ProductId == 135)
            },
            new OrderDTO
            {
                Amount = 4,
                ProductSupplier = productList.Single(item => item.ProductId == 136)
            }
        };
        
        ProductOrderDTO productOrder = new() {Orders = orderList};
        
        var action = () => _fixture.StoreApplicationFailure.OrderProducts(productOrder);
        var exception = Assert.Throws<StoreException>(action);
        Assert.Equal("One or more errors occurred. (An unexpected error occurred while executing the order!)", 
            exception.Message);
    }
}