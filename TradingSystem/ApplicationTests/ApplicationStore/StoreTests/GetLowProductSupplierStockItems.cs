using System;
using Application.Exceptions;
using Application.Store;
using Xunit;

namespace ApplicationTests.ApplicationStore.StoreTests;

[Collection("ApplicationTestCollection")]
public class GetLowProductSupplierStockItems
{
    private readonly DatabaseFixture _fixture;

    public GetLowProductSupplierStockItems(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public void Success_Get_Low_Product_Supplier_StockItems()
    {
        ProductSupplierStockItemDTO expecetedDto = new()
        {
            ProductId = 140,
            Barcode = 10000139,
            ProductName = "Milka Alpen Tafelschokolade",
            PurchasePrice = 5.78,
            SupplierId = 6,
            SupplierName = "Betz Lange",
            StockItem = new StockItemDTO
            {
                ItemId = 3,
                Amount = 6,
                MaxStock = 50,
                MinStock = 10,
                SalesPrice = 8.09
            }
        };
        var result = 
            _fixture.StoreApplication.GetLowProductSupplierStockItems();
        Assert.Equal(13, result.Count);
        Assert.Contains(result, item => item.Equals(expecetedDto));
    }

    [Fact]
    public void Failure_Get_Low_Product_Supplier_StockItems()
    {
        var action = () => _fixture.StoreApplicationFailure.GetLowProductSupplierStockItems();
        var exception = Assert.Throws<StoreException>(action);
        Assert.Equal(
            "One or more errors occurred. (An unexpected error occurred while receiving the product stock item list!)", 
            exception.Message);
    }
    
}