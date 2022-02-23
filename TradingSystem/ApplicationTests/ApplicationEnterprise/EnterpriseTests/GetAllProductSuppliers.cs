using Application.Exceptions;
using Application.Store;
using Xunit;

namespace ApplicationTests.ApplicationEnterprise.EnterpriseTests;

[Collection("ApplicationTestCollection")]
public class GetAllProductSuppliers
{
    private readonly DatabaseFixture _fixture;

    public GetAllProductSuppliers(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public void Success_Get_All_Product_Supplier()
    {
        const long storeId = 2;
        ProductSupplierDTO expecetedDto = new()
        {
            ProductId = 25,
            Barcode = 10000024,
            ProductName = "Coca Cola",
            PurchasePrice = 20.65,
            SupplierId = 1,
            SupplierName = "Schegel"
        };
        var result =
            _fixture.EnterpriseApplication.GetAllProductSuppliers(storeId);
        Assert.Equal(46, result.Count);
        Assert.Contains(result, item => item.Equals(expecetedDto));
    }

    [Fact]
    public void Failure_Get_All_Product_Supplier()
    {
        const long storeId = -1;
        var action = () => _fixture.EnterpriseApplication.GetAllProductSuppliers(storeId);
        var exception = Assert.Throws<EnterpriseException>(action);
        Assert.Equal("An unexpected error occurred while receiving the product supplier list!", 
            exception.Message);
    }
}