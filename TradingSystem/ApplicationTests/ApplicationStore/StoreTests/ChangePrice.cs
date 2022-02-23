using Application.Exceptions;
using Xunit;

namespace ApplicationTests.ApplicationStore.StoreTests;

[Collection("ApplicationTestCollection")]
public class ChangePrice
{
    private readonly DatabaseFixture _fixture;

    public ChangePrice(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Success_Change_Price()
    {
        const long stockitemId = 4;
        const double newSalesPrice = 35.0;
        _fixture.StoreApplication.ChangePrice(stockitemId, newSalesPrice);
        var result = _fixture.Context.StockItems.Find(stockitemId)!;
        Assert.Equal(stockitemId, result.Id);
        Assert.Equal(newSalesPrice, result.SalesPrice);
    }
    
    [Fact]
    public void Failure_Change_Price()
    {
        const long stockitemId = 300;
        const double newSalesPrice = 35.0;
        var action = () => _fixture.StoreApplication.ChangePrice(stockitemId, newSalesPrice);
        var exception = Assert.Throws<StoreException>(action);
        Assert.Equal("An unexpected error occurred while changing the price!", exception.Message);
    }
}