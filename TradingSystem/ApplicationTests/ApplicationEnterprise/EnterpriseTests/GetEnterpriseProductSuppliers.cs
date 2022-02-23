using Application.Exceptions;
using Xunit;

namespace ApplicationTests.ApplicationEnterprise.EnterpriseTests;

[Collection("ApplicationTestCollection")]
public class GetEnterpriseProductSuppliers
{
    private readonly DatabaseFixture _fixture;

    public GetEnterpriseProductSuppliers(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public void Success_Get_Enterprise_Product_Suppliers()
    {
        var result = _fixture.EnterpriseApplication.GetEnterpriseProductSuppliers();
        Assert.Equal(6, result.Count);
        Assert.Collection(result,
            supplier => 
            {
                Assert.Equal(1, supplier.SupplierId);
                Assert.Equal("Schegel", supplier.SupplierName);
            },
            supplier => 
            {
                Assert.Equal(2, supplier.SupplierId);
                Assert.Equal("Kaufmann", supplier.SupplierName);
            },
            supplier => 
            {
                Assert.Equal(3, supplier.SupplierId);
                Assert.Equal("Lutz GmbH", supplier.SupplierName);
            },
            supplier => 
            {
                Assert.Equal(4, supplier.SupplierId);
                Assert.Equal("Scheffler GmbH", supplier.SupplierName);
            },
            supplier => 
            {
                Assert.Equal(5, supplier.SupplierId);
                Assert.Equal("Thiele", supplier.SupplierName);
            },
            supplier => 
            {
                Assert.Equal(6, supplier.SupplierId);
                Assert.Equal("Betz Lange", supplier.SupplierName);
            });
    }

    [Fact]
    public void Failure_Get_Enterprise_Product_Suppliers()
    {
        var action = () => _fixture.EnterpriseApplicationFailed.GetEnterpriseProductSuppliers();
        var exception = Assert.Throws<EnterpriseException>(action);
        Assert.Equal("An unexpected error occurred while receiving the product supplier list!", 
            exception.Message);
    }
}