using Application.Exceptions;
using Xunit;

namespace ApplicationTests.ApplicationEnterprise.ReportingTests;

[Collection("ApplicationTestCollection")]
public class GetEnterpriseStockReport
{
    private readonly DatabaseFixture _fixture;

    public GetEnterpriseStockReport(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public void Success_Enterprise_Report()
    {
        const long enterpriseId = 1;
        var result = _fixture.EnterpriseApplication.GetEnterpriseStockReport(enterpriseId);
        Assert.Equal(enterpriseId, result.EnterpriseId);
        Assert.Equal("CocomeSystem GmbH & Co. KG", result.EnterpriseName);
        Assert.Equal(3, result.StoreReports.Count);
        Assert.Contains(result.StoreReports,  report => report.StoreId == 1 && report.StockItems.Count == 63);
        Assert.Contains(result.StoreReports,  report => report.StoreId == 2 && report.StockItems.Count == 46);
        Assert.Contains(result.StoreReports,  report => report.StoreId == 3 && report.StockItems.Count == 31);
       
    }

    [Fact]
    public void Failure_Enterprise_Report()
    {
        const long enterpriseId = -1;
        var action = 
            () => _fixture.EnterpriseApplication.GetEnterpriseStockReport(enterpriseId);
        var exception = Assert.Throws<EnterpriseException>(action);
        Assert.Equal("An unexpected error occurred while creating the enterprise stock report!", 
            exception.Message);
    }
}