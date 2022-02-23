using Application.Exceptions;
using Application.Store;
using Xunit;

namespace ApplicationTests.ApplicationStore.StoreTests;

[Collection("ApplicationTestCollection")]
public class GetAllProductSuppliers
{
    private readonly DatabaseFixture _fixture;

    public GetAllProductSuppliers(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Success_Get_All_Product_Suppliers()
    {
        ProductSupplierDTO expecetedDto = new()
        {
            ProductId = 56,
            Barcode = 10000055,
            ProductName = "ZOTT Monte Schoko",
            PurchasePrice = 9.47,
            SupplierId = 3,
            SupplierName = "Lutz GmbH"
        };
        var result =
            _fixture.StoreApplication.GetAllProductSuppliers();
        Assert.Equal(63, result.Count);
        Assert.Contains(result, item => item.Equals(expecetedDto));
    }
    
    [Fact]
    public void Failure_Get_All_Product_Supplier()
    {
        var action = () => _fixture.StoreApplicationFailure.GetAllProductSuppliers();
        var exception = Assert.Throws<StoreException>(action);
        Assert.Equal(
            "One or more errors occurred. (An unexpected error occurred while receiving the product supplier list!)", 
            exception.Message);
    }
}