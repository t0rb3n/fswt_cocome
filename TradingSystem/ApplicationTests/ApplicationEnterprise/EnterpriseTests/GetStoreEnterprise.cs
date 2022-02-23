using Application.Enterprise;
using Application.Exceptions;
using Application.Store;
using Xunit;

namespace ApplicationTests.ApplicationEnterprise.EnterpriseTests;

[Collection("ApplicationTestCollection")]
public class GetStoreEnterprise
{
    private readonly DatabaseFixture _fixture;

    public GetStoreEnterprise(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public void Success_Get_Store_Enterprise()
    {
        const long storeId = 1;
        StoreEnterpriseDTO expecetedDto = new()
        {
            StoreId = 1,
            StoreName = "Cocome WI",
            Location = "Wiesbaden",
            Enterprise = new EnterpriseDTO
            {
                EnterpriseId = 1,
                EnterpriseName = "CocomeSystem GmbH & Co. KG"
            }
        };
        var result = _fixture.EnterpriseApplication.GetStoreEnterprise(storeId);
        Assert.Equal(expecetedDto, result);
    }

    [Fact]
    public void Failed_Get_Store_Enterprise()
    {
        const long storeId = -1;
        var action = () => _fixture.EnterpriseApplication.GetStoreEnterprise(storeId);
        var exception = Assert.Throws<EnterpriseException>(action);
        Assert.Equal("Store could not be found!", exception.Message);
    }
}