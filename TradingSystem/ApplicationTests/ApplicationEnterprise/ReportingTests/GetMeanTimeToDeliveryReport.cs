using System;
using Application.Exceptions;
using Xunit;

namespace ApplicationTests.ApplicationEnterprise.ReportingTests;

[Collection("ApplicationTestCollection")]
public class GetMeanTimeToDeliveryReport
{
    private readonly DatabaseFixture _fixture;

    public GetMeanTimeToDeliveryReport(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public void Success_Mean_Time_Report()
    {
        const long enterpriseId = 1;
        var expectedMtS01 = TimeSpan.FromTicks(
            DateTime.Parse("2022-02-27 16:16:32").Ticks - DateTime.Parse("2022-02-19 17:10:12").Ticks);
        var expectedMtS02 = TimeSpan.FromTicks(0);
        var expectedMtS03 = TimeSpan.FromTicks(0);
        var expectedMtS04 = TimeSpan.FromTicks(
            ((DateTime.Parse("2022/02/25 05:29:59").Ticks - DateTime.Parse("2022/02/02 20:35:24").Ticks)
            + (DateTime.Parse("2022/02/27 15:53:33").Ticks - DateTime.Parse("2022/02/16 10:47:30").Ticks))
            / 2);
        var expectedMtS05 = TimeSpan.FromTicks(
            ((DateTime.Parse("2022/02/14 01:49:59").Ticks - DateTime.Parse("2022/02/05 19:58:42").Ticks) 
             + (DateTime.Parse("2022/02/25 06:12:01").Ticks - DateTime.Parse("2022/02/13 08:35:31").Ticks))
            / 2);
        var expectedMtS06 = TimeSpan.FromTicks(0);
        
        var result = 
            _fixture.EnterpriseApplication.GetMeanTimeToDeliveryReport(enterpriseId);
        Assert.Equal(6, result.Count);
        Assert.Collection(result, 
            supplier => 
            {
                Assert.Equal(1, supplier.SupplierId);
                Assert.Equal(expectedMtS01, supplier.MeanTime);
            },
            supplier => 
            {
                Assert.Equal(2, supplier.SupplierId);
                Assert.Equal(expectedMtS02, supplier.MeanTime);
            },
            supplier => 
            {
                Assert.Equal(3, supplier.SupplierId);
                Assert.Equal(expectedMtS03, supplier.MeanTime);
            },
            supplier => 
            {
                Assert.Equal(4, supplier.SupplierId);
                Assert.Equal(expectedMtS04, supplier.MeanTime);
            },
            supplier => 
            {
                Assert.Equal(5, supplier.SupplierId);
                Assert.Equal(expectedMtS05, supplier.MeanTime);
            },
            supplier => 
            {
                Assert.Equal(6, supplier.SupplierId);
                Assert.Equal(expectedMtS06, supplier.MeanTime);
            }
        );
    }

    [Fact]
    public void Failure_Mean_Time_Report()
    {
        const long enterpriseId = -1;
        var action = 
            () =>  _fixture.EnterpriseApplication.GetMeanTimeToDeliveryReport(enterpriseId);
        var exception = Assert.Throws<EnterpriseException>(action);
        Assert.Equal("An unexpected error occurred while calculating the mean delivery time of a supplier!", 
            exception.Message);
    }
}