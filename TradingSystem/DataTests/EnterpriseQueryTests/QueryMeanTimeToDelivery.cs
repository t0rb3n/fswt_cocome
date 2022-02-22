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
    public void Mean_Time_Success_01()
    {
        const long enterpriseId = 1;
        const long supplierId = 1;
        const long excepted = 0;
        
        var result = _enterpriseQuery.QueryMeanTimeToDelivery(supplierId, enterpriseId, _fixture.Context);
        Assert.Equal(excepted, result);
    }
    
    [Fact]
    public void Mean_Time_Success_02()
    {
        const long enterpriseId = 1;
        const long supplierId = 2; //Kaufmann
        var excepted = ((DateTime.Parse("2022-02-27 16:16:32").Ticks - DateTime.Parse("2022-02-19 17:10:12").Ticks) +
                        (DateTime.Parse("2022-02-14 01:49:59").Ticks - DateTime.Parse("2022-02-05 19:58:42").Ticks)) / 2;

        var result = _enterpriseQuery.QueryMeanTimeToDelivery(supplierId, enterpriseId, _fixture.Context);
        Assert.Equal(excepted, result);
    }
    
    [Fact]
    public void Mean_Time_No_Success_01()
    {
        const long enterpriseId = 1;
        const long supplierId = 23;
        
        Action action = () => _enterpriseQuery.QueryMeanTimeToDelivery(supplierId, enterpriseId, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal($"Suppliers with the id '{supplierId}' could not be found!",
            exception.Message);
    }
    
    [Fact]
    public void Mean_Time_No_Success_02()
    {
        const long enterpriseId = 23;
        const long supplierId = 2;

        Action action = () => _enterpriseQuery.QueryMeanTimeToDelivery(supplierId, enterpriseId, _fixture.Context);
        var exception = Assert.Throws<ItemNotFoundException>(action);
        Assert.Equal($"Enterprise with the id '{enterpriseId}' could not be found!", exception.Message);
    }
}