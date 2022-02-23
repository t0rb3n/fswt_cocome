using System;
using Application.Exceptions;
using Xunit;

namespace ApplicationTests.ApplicationEnterprise.EnterpriseTests;

[Collection("ApplicationTestCollection")]
public class GetProductStockItem
{
    private readonly DatabaseFixture _fixture;

    public GetProductStockItem(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Success_Get_Product_StockItem()
    {
        const long storeId = 1;
        const long barcode = 10000135; //Perwoll Waschmittel
        var result = _fixture.EnterpriseApplication.GetProductStockItem(barcode, storeId);
        Assert.Equal(barcode, result.Barcode);
        Assert.Equal(136, result.ProductId);
        Assert.Equal(63, result.StockItem.ItemId);
    }
    
    [Fact]
    public void Failure_Get_Product_StockItem_01()
    {
        const long storeId = 23;
        const long barcode = 10000135; //Perwoll Waschmittel
        var action = () => _fixture.EnterpriseApplication.GetProductStockItem(barcode, storeId);
        var exception = Assert.Throws<EnterpriseException>(action);
        Assert.Equal("Product stock item could not be found!", exception.Message);
    }
    
    [Fact]
    public void Failure_Get_Product_StockItem_02()
    {
        const long storeId = 23;
        const long barcode = 10020135;
        var action = () => _fixture.EnterpriseApplication.GetProductStockItem(barcode, storeId);
        var exception = Assert.Throws<EnterpriseException>(action);
        Assert.Equal("Product stock item could not be found!", exception.Message);
    }
    
    [Fact]
    public void Failure_Get_Product_StockItem_03()
    {
        const long storeId = 1;
        const long barcode = 10020135;
        var action = () => _fixture.EnterpriseApplication.GetProductStockItem(barcode, storeId);
        var exception = Assert.Throws<EnterpriseException>(action);
        Assert.Equal("Product stock item could not be found!", exception.Message);
    }
}