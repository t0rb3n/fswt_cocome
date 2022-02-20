using System;
using Data.Enterprise;
using Data.Exceptions;
using Xunit;

namespace DataTests.EnterpriseQueryTests;

[Collection("DataTestCollection")]
public class QueryMeanTimeToDelivery
{
    private readonly DatabaseFixture _fixture;
    private readonly IEnterpriseQuery _enterpriseQuery;

    public QueryMeanTimeToDelivery(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _enterpriseQuery = new EnterpriseQuery();
    }

    [Fact]
    public void Mean_Time_Success()
    {
        const long excepted = 1583870000000;
        var enterprise = new Enterprise
        {
            Id = 1,
            Name = "CocomeSystem GmbH & Co. KG"
        };
        var supplier = new ProductSupplier
        {
            Id = 1,
            Name = "Schegel"
        };
        var result = _enterpriseQuery.QueryMeanTimeToDelivery(supplier, enterprise, _fixture.Context);
        Assert.Equal(excepted, result);
    }
    
    [Fact]
    public void Mean_Time_No_Success_01()
    {
        var enterprise = new Enterprise
        {
            Id = 1,
            Name = "CocomeSystem GmbH & Co. KG"
        };
        var supplier = new ProductSupplier
        {
            Id = 2,
            Name = "Kaufmann"
        };
        
        Action action = () => _enterpriseQuery.QueryMeanTimeToDelivery(supplier, enterprise, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal($"The average time could not be calculated because an item was not found by the query!",
            exception.Message);
    }
    [Fact]
    public void Mean_Time_No_Success_02()
    {
        var enterprise = new Enterprise
        {
            Id = 23,
            Name = "No Enterprise System"
        };
        var supplier = new ProductSupplier
        {
            Id = 2,
            Name = "Kaufmann"
        };
        
        Action action = () => _enterpriseQuery.QueryMeanTimeToDelivery(supplier, enterprise, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal($"The average time could not be calculated because an item was not found by the query!",
            exception.Message);
    }
}