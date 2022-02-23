using Application.Exceptions;
using Application.Store;
using Xunit;

namespace ApplicationTests.ApplicationEnterprise.EnterpriseTests;

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
        const long storeId = 3;
        ProductSupplierStockItemDTO expecetedDto = new()
        {
            ProductId = 81,
            Barcode = 10000080,
            ProductName = "Toilettenpapier",
            PurchasePrice = 8,
            SupplierId = 4,
            SupplierName = "Scheffler GmbH",
            StockItem = new StockItemDTO
            {
                ItemId = 134,
                Amount = 14,
                MaxStock = 50,
                MinStock = 10,
                SalesPrice = 11.2
            }
        };
        var result = 
            _fixture.EnterpriseApplication.GetAllProductSupplierStockItems(storeId);
        Assert.Equal(31, result.Count);
        Assert.Contains(result, item => item.Equals(expecetedDto));
    }

    [Fact]
    public void Failure_Get_All_Product_Supplier_StockItems()
    {
        const long storeId = -1;
        var action = () => _fixture.EnterpriseApplication.GetAllProductSupplierStockItems(storeId);
        var exception = Assert.Throws<EnterpriseException>(action);
        Assert.Equal("An unexpected error occurred while receiving the product supplier stock item list!", 
            exception.Message);
    }
}