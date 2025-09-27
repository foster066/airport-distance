using AviaExpress.AirportDistance.Core.DistanceAggregate;
using AviaExpress.AirportDistance.Core.DistanceAggregate.Exceptions;
using AviaExpress.AirportDistance.Core.DistanceAggregate.ExternalService;
using AviaExpress.AirportDistance.Core.Interfaces;
using AviaExpress.AirportDistance.Core.Services;
using AviaExpress.AirportDistance.SharedKernel;
using FakeItEasy;

namespace AviaExpress.AirportDistance.UnitTests.Core.Services;

public class DistanceCalculatorServiceUnitTest
{
    private readonly ICache _cache;
    private readonly IDistanceCalculatorService _service;
    private readonly ICoordinatesCalculator _coordinatesCalculator;

    public DistanceCalculatorServiceUnitTest()
    {
        _cache = A.Fake<ICache>();
        IAirportInformationService airportInfoService = A.Fake<IAirportInformationService>();
        _coordinatesCalculator = A.Fake<ICoordinatesCalculator>();
        _service = new DistanceCalculatorService(_cache, airportInfoService, _coordinatesCalculator);
    }

    [Fact]
    public async Task CalculateDistanceBetweenAirports_FirstAirportInfoNotFound_AirportNotFoundException()
    {
        // Arrange
        const string firstAirportName = nameof(firstAirportName);
        const string secondAirportName = nameof(secondAirportName);
        CancellationToken cancellationToken = CancellationToken.None;
            
        A.CallTo(() => _cache.GetOrAdd(firstAirportName, A<Func<string, CancellationToken, Task<AirportLocation?>>>._, A<CancellationToken>._))
            .Returns(Task.FromResult((AirportLocation?)null));

        A.CallTo(() => _cache.GetOrAdd(secondAirportName, A<Func<string, CancellationToken, Task<AirportLocation?>>>._, A<CancellationToken>._))
            .Returns(Task.FromResult<AirportLocation?> (new AirportLocation()));

        // Act & Assert
        await Assert.ThrowsAsync<AirportNotFoundException>(() => _service.CalculateDistanceBetweenAirports(firstAirportName, secondAirportName, cancellationToken));
    }

    [Fact]
    public async Task CalculateDistanceBetweenAirports_TwoAirportsInfoProvided_CallsCalculator()
    {
        // Arrange
        const string firstAirportName = nameof(firstAirportName);
        const string secondAirportName = nameof(secondAirportName);
        CancellationToken cancellationToken = CancellationToken.None;
        AirportLocation firstAirportInfo = new()
        {
            Latitude = 1,
            Longitude = 2
        };
        AirportLocation secondAirportInfo = new()
        {
            Latitude = 3,
            Longitude = 4
        };
        decimal expectedDistance = 123;


        A.CallTo(() => _cache.GetOrAdd(firstAirportName, A<Func<string, CancellationToken, Task<AirportLocation?>>>._, A<CancellationToken>._))
            .Returns(Task.FromResult<AirportLocation?>(firstAirportInfo));

        A.CallTo(() => _cache.GetOrAdd(secondAirportName, A<Func<string, CancellationToken, Task<AirportLocation?>>>._, A<CancellationToken>._))
            .Returns(Task.FromResult<AirportLocation?>(secondAirportInfo));
        A.CallTo(() => _coordinatesCalculator.CalculateDistance(
            A<CoordinatesPoint>.That.Matches(i =>
                i.Latitude == firstAirportInfo.Latitude &&
                i.Longitude == firstAirportInfo.Longitude),
            A<CoordinatesPoint>.That.Matches(i =>
                i.Latitude == secondAirportInfo.Latitude &&
                i.Longitude == secondAirportInfo.Longitude))).Returns(expectedDistance);

        // Act
        decimal actualDistance = await _service.CalculateDistanceBetweenAirports(firstAirportName, secondAirportName, cancellationToken);

        // Assert
        Assert.Equal(expectedDistance, actualDistance);
    }
}