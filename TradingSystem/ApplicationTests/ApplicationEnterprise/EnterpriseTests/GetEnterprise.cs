using Application.Enterprise;
using Application.Exceptions;
using Xunit;

namespace ApplicationTests.ApplicationEnterprise.EnterpriseTests;

[Collection("ApplicationTestCollection")]
public class GetEnterprise
{
    private readonly DatabaseFixture _fixture;

    public GetEnterprise(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public void Success_Get_Enterprise()
    {
        EnterpriseDTO expecetedDto = new()
        {
            EnterpriseId = 1,
            EnterpriseName = "CocomeSystem GmbH & Co. KG"
        };
        var result = _fixture.EnterpriseApplication.GetEnterprise();
        Assert.Equal(expecetedDto, result);

    }

    [Fact]
    public void Failure_Get_Enterprise()
    {
        var action = () => _fixture.EnterpriseApplicationFailure.GetEnterprise();
        var exception = Assert.Throws<EnterpriseException>(action);
        Assert.Equal("Enterprise could not be found!", exception.Message);
    }
}