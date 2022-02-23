using System;
using System.Collections.Generic;
using Application.Exceptions;
using Application.Store;
using Xunit;

namespace ApplicationTests.ApplicationStore.CashDeskConnectorTests;

[Collection("ApplicationTestCollection")]
public class BookSale
{
    private readonly DatabaseFixture _fixture;

    public BookSale(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Success_Book_Sale()
    {
        //Martini, 2x Head & Shoulders Shampoo, Bio-Tafeläpfel, Dr. Oetker Ristorante Pizza
        long[] barcodes = {10000103, 10000037, 10000037, 10000132, 10000041};
        var saleList = new List<ProductStockItemDTO>();

        foreach (var code in barcodes)
        {
            saleList.Add(_fixture.StoreApplication.GetProductStockItem(code));
        }

        var sales = new SaleDTO()
        {
            Date = DateTime.Now,
            Products = saleList
        };

        _fixture.StoreApplication.BookSale(sales);
        var result = _fixture.StoreApplication.GetProductStockItem(barcodes[0]);
        Assert.Equal(saleList[0].StockItem.Amount - 1, result.StockItem.Amount);
        
        result = _fixture.StoreApplication.GetProductStockItem(barcodes[1]);
        Assert.Equal(saleList[1].StockItem.Amount - 2, result.StockItem.Amount);
        
        result = _fixture.StoreApplication.GetProductStockItem(barcodes[3]);
        Assert.Equal(saleList[3].StockItem.Amount - 1, result.StockItem.Amount);
        
        result = _fixture.StoreApplication.GetProductStockItem(barcodes[4]);
        Assert.Equal(saleList[4].StockItem.Amount - 1, result.StockItem.Amount);
    }

    [Fact]
    public void Failure_Book_Sale_01()
    {
        var saleList = new List<ProductStockItemDTO>() {new()};

        var sales = new SaleDTO()
        {
            Date = DateTime.Now,
            Products = saleList
        };
        
        var action = () => _fixture.StoreApplication.BookSale(sales);
        var exception = Assert.Throws<StoreException>(action);
        Assert.Equal("An unexpected error occurred while booking the stock items!", exception.Message);

    }

    [Fact]
    public void Failure_Book_Sale_02()
    {
        var sales = new SaleDTO()
        {
            Date = DateTime.Now
        };

        var action = () => _fixture.StoreApplication.BookSale(sales);
        var exception = Assert.Throws<StoreException>(action);
        Assert.Equal("The sale does not include product sales!", exception.Message);
    }
}