using Application.Exceptions;
using Application.Store;
using Xunit;

namespace ApplicationTests.ApplicationStore.StoreTests;

[Collection("ApplicationTestCollection")]
public class GetAllProductSupplierStockItems
{
    private readonly DatabaseFixture _fixture;

    public GetAllProductSupplierStockItems(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public void Success_Get_All_Product_Supplier_StockItems()
    {
        ProductSupplierStockItemDTO expecetedDto = new()
        {
            ProductId = 58,
            Barcode = 10000057,
            ProductName = "KÖLLN Müsli",
            PurchasePrice = 2.82,
            SupplierId = 3,
            SupplierName = "Lutz GmbH",
            StockItem = new StockItemDTO
            {
                ItemId = 37,
                Amount = 14,
                MaxStock = 50,
                MinStock = 10,
                SalesPrice = 3.95
            }
        };
        var result = 
            _fixture.StoreApplication.GetAllProductSupplierStockItems();
        Assert.Equal(63, result.Count);
        Assert.Contains(result, item => item.Equals(expecetedDto));
    }

    [Fact]
    public void Failure_Get_All_Product_Supplier_StockItems()
    {
        var action = () => _fixture.StoreApplicationFailure.GetAllProductSupplierStockItems();
        var exception = Assert.Throws<StoreException>(action);
        Assert.Equal(
            "One or more errors occurred. (An unexpected error occurred while receiving the product supplier stock item list!)", 
            exception.Message);
    }
}