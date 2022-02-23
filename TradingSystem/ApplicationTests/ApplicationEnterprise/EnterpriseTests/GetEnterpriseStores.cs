using Application.Exceptions;
using Xunit;

namespace ApplicationTests.ApplicationEnterprise.EnterpriseTests;

[Collection("ApplicationTestCollection")]
public class GetEnterpriseStores
{
    private readonly DatabaseFixture _fixture;

    public GetEnterpriseStores(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public void Success_Get_Enterprise_Stores()
    {
        var result = _fixture.EnterpriseApplication.GetEnterpriseStores();
        Assert.Equal(3, result.Count);
        Assert.Collection(result,
            store => 
            {
                Assert.Equal(1, store.StoreId);
                Assert.Equal("Cocome WI", store.StoreName);
            },
            store => 
            {
                Assert.Equal(2, store.StoreId);
                Assert.Equal("Cocome F", store.StoreName);
            },
            store =>
            {
                Assert.Equal(3, store.StoreId);
                Assert.Equal("Cocome MZ", store.StoreName);
            });
    }

    [Fact]
    public void Failure_Get_Enterprise_Stores()
    {
        var action = () => _fixture.EnterpriseApplicationFailed.GetEnterpriseStores();
        var exception = Assert.Throws<EnterpriseException>(action);
        Assert.Equal("An unexpected error occurred while receiving the store list!", exception.Message);
    }
}