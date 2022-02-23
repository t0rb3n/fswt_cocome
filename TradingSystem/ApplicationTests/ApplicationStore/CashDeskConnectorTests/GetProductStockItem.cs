using Application.Exceptions;
using Application.Store;
using Xunit;

namespace ApplicationTests.ApplicationStore.CashDeskConnectorTests;

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
        var expectedDto = new ProductStockItemDTO()
        {
            ProductId = 52,
            Barcode = 10000051,
            ProductName = "Wolfsbarsch",
            PurchasePrice = 12.65,
            StockItem = new StockItemDTO()
            {
                ItemId = 34,
                Amount = 18,
                MaxStock = 50,
                MinStock = 10,
                SalesPrice = 17.71
            }
        };
        var result = _fixture.StoreApplication.GetProductStockItem(expectedDto.Barcode);
        Assert.Equal(expectedDto, result);
     
    }
    
    [Fact]
    public void Failure_Get_Product_StockItem_01()
    {
        var expectedDto = new ProductStockItemDTO()
        {
            ProductId = 42,
            Barcode = 10000041,
            ProductName = "Dr. Oetker Ristorante Pizza",
            PurchasePrice = 20.61,
            StockItem = new StockItemDTO()
            {
                ItemId = 12,
                Amount = 14,
                MaxStock = 50,
                MinStock = 10,
                SalesPrice = 28.85
            }
        };
        var action = () => 
            _fixture.StoreApplicationFailure.GetProductStockItem(expectedDto.Barcode);
        var exception = Assert.Throws<StoreException>(action);
        Assert.Equal("Product stock item could not be found!", exception.Message);
    }
    
    [Fact]
    public void Failure_Get_Product_StockItem_02()
    {
        const long barcode = 10020135;
        var action = () => _fixture.StoreApplicationFailure.GetProductStockItem(barcode);
        var exception = Assert.Throws<StoreException>(action);
        Assert.Equal("Product stock item could not be found!", exception.Message);
    }
    
    [Fact]
    public void Failure_Get_Product_StockItem_03()
    {
        const long barcode = 10020135;
        var action = () => _fixture.StoreApplication.GetProductStockItem(barcode);
        var exception = Assert.Throws<StoreException>(action);
        Assert.Equal("Product stock item could not be found!", exception.Message);
    }
}