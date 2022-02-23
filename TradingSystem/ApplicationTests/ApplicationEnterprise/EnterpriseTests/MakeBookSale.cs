using System;
using System.Collections.Generic;
using Application.Exceptions;
using Application.Store;
using Xunit;

namespace ApplicationTests.ApplicationEnterprise.EnterpriseTests;

[Collection("ApplicationTestCollection")]
public class MakeBookSale
{
    private readonly DatabaseFixture _fixture;

    public MakeBookSale(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Success_Make_Book_Sale()
    {
        const long storeId = 1;
        //Hot Dog, ZOTT Monte Schoko, 2x Weizenbrot, Coca Cola
        long[] barcodes = {10000076, 10000005, 10000089, 10000089, 10000043};
        var saleList = new List<ProductStockItemDTO>();

        foreach (var code in barcodes)
        {
            saleList.Add(_fixture.EnterpriseApplication.GetProductStockItem(code, storeId));
        }

        var sales = new SaleDTO()
        {
            Date = DateTime.Now
        };

        sales.Products = saleList;
        _fixture.EnterpriseApplication.MakeBookSale(sales);
        var result = _fixture.EnterpriseApplication.GetProductStockItem(barcodes[0], storeId);
        Assert.Equal(saleList[0].StockItem.Amount - 1, result.StockItem.Amount);
        result = _fixture.EnterpriseApplication.GetProductStockItem(barcodes[1], storeId);
        Assert.Equal(saleList[1].StockItem.Amount - 1, result.StockItem.Amount);
        result = _fixture.EnterpriseApplication.GetProductStockItem(barcodes[2], storeId);
        Assert.Equal(saleList[2].StockItem.Amount - 2, result.StockItem.Amount);
        result = _fixture.EnterpriseApplication.GetProductStockItem(barcodes[4], storeId);
        Assert.Equal(saleList[4].StockItem.Amount - 1, result.StockItem.Amount);
    }

    [Fact]
    public void Failure_Make_Book_Sale_01()
    {
        var saleList = new List<ProductStockItemDTO>()
        {
            new()
        };

        var sales = new SaleDTO()
        {
            Date = DateTime.Now
        };

        sales.Products = saleList;

        var action = () => _fixture.EnterpriseApplication.MakeBookSale(sales);
        var exception = Assert.Throws<EnterpriseException>(action);
        Assert.Equal("An unexpected error occurred while booking the stock items!", exception.Message);

    }

    [Fact]
    public void Failure_Make_Book_Sale_02()
    {
        var sales = new SaleDTO()
        {
            Date = DateTime.Now
        };

        var action = () => _fixture.EnterpriseApplication.MakeBookSale(sales);
        var exception = Assert.Throws<EnterpriseException>(action);
        Assert.Equal("The sale does not include product sales!", exception.Message);
    }

}