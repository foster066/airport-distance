using AviaExpress.AirportDistance.Core.DistanceAggregate;
using AviaExpress.AirportDistance.Core.Interfaces;
using AviaExpress.AirportDistance.Core.Services;

namespace AviaExpress.AirportDistance.UnitTests.Core.Services;

public class MilesCoordinatesCalculatorUnitTest
{
    private readonly ICoordinatesCalculator _calculator;
    public MilesCoordinatesCalculatorUnitTest()
    {
        _calculator = new MilesCoordinatesCalculator();
    }

    // TODO add more data for validation
    [Theory]
    [InlineData(55.612573, 37.282105, 55.415055, 37.900720, 27.77)] 
    public void GetDistance_ValidCoordinates_ReturnsDistance(decimal latitude1, decimal longitude1, decimal latitude2, decimal longitude2, decimal expectedDistance)
    {
        // Act
        decimal distance = _calculator.CalculateDistance(new CoordinatesPoint
            {
                Latitude = latitude1,
                Longitude = longitude1,
            },
            new CoordinatesPoint
            {
                Latitude = latitude2,
                Longitude = longitude2,
            });

        // Assert
        Assert.Equal(expectedDistance, distance);
    }
}