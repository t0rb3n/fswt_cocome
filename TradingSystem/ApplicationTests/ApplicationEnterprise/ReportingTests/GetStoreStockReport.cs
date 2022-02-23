using Application.Exceptions;
using Xunit;

namespace ApplicationTests.ApplicationEnterprise.ReportingTests;

[Collection("ApplicationTestCollection")]
public class GetStoreStockReport
{
    private readonly DatabaseFixture _fixture;

    public GetStoreStockReport(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public void Success_Store_Report()
    {
        const long storeId = 1;
        var result = _fixture.EnterpriseApplication.GetStoreStockReport(storeId);
        Assert.Equal(storeId,result.StoreId);
        Assert.Equal("Cocome WI", result.StoreName);
        Assert.Equal("Wiesbaden", result.Location);
        Assert.Equal(63, result.StockItems.Count);
        Assert.Contains(result.StockItems, filter => 
            filter.StockItem.ItemId == 1 &&
            filter.ProductId == 77 &&
            filter.ProductName == "Hot Dog" &&
            filter.SupplierName == "Scheffler GmbH");
    }
    
    [Fact]
    public void Failure_Store_Report()
    {
        const long storeId = -1;
        var action = () => _fixture.EnterpriseApplication.GetStoreStockReport(storeId);
        var exception = Assert.Throws<EnterpriseException>(action);
        Assert.Equal("An unexpected error occurred while creating the store stock report!", 
            exception.Message);
    }
}