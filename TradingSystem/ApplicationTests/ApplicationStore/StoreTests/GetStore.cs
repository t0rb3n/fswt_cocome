using Application.Enterprise;
using Application.Exceptions;
using Application.Store;
using Xunit;

namespace ApplicationTests.ApplicationStore.StoreTests;

[Collection("ApplicationTestCollection")]
public class GetStore
{
    private readonly DatabaseFixture _fixture;

    public GetStore(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Success_Get_Store()
    {
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
        var result = _fixture.StoreApplication.GetStore();
        Assert.Equal(expecetedDto, result);
    }
    
    [Fact]
    public void Failure_Get_Store()
    {
        var action = () => _fixture.StoreApplicationFailure.GetStore();
        var exception = Assert.Throws<StoreException>(action);
        Assert.Equal("Store could not be found!", exception.Message);
    }
}