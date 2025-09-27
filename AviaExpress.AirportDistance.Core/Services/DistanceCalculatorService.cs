using AviaExpress.AirportDistance.Core.DistanceAggregate.ExternalService;
using AviaExpress.AirportDistance.Core.Interfaces;
using AviaExpress.AirportDistance.SharedKernel;
using System.Text;
using AviaExpress.AirportDistance.Core.DistanceAggregate;
using AviaExpress.AirportDistance.Core.DistanceAggregate.Exceptions;

namespace AviaExpress.AirportDistance.Core.Services;

public class DistanceCalculatorService : IDistanceCalculatorService
{
    private readonly ICache _cache;
    private readonly IAirportInformationService _airportInfoService;
    private readonly ICoordinatesCalculator _coordinatesCalculator;

    public DistanceCalculatorService(ICache cache, IAirportInformationService airportInfoService, ICoordinatesCalculator coordinatesCalculator)
    {
        _cache = cache;
        _airportInfoService = airportInfoService;
        _coordinatesCalculator = coordinatesCalculator;
    }

    public async Task<decimal> CalculateDistanceBetweenAirports(string firstAirport, string secondAirport, CancellationToken cancellationToken)
    {
        Task<AirportLocation?> firstAirportInfoTask = _cache.GetOrAdd(firstAirport, (key, token) => _airportInfoService.GetAirportInfo(key, token), cancellationToken);
        Task<AirportLocation?> secondAirportInfoTask = _cache.GetOrAdd(secondAirport, (key, token) => _airportInfoService.GetAirportInfo(key, token), cancellationToken);

        await Task.WhenAll(firstAirportInfoTask, secondAirportInfoTask); // use safe WhenAll

        AirportLocation? firstAirportInfo = await firstAirportInfoTask;
        AirportLocation? secondAirportInfo = await secondAirportInfoTask;

        if (firstAirportInfo is null || secondAirportInfo is null)
        {
            throw new AirportNotFoundException(CreateErrorMessageString(firstAirportInfo is null, firstAirport, secondAirportInfo is null, secondAirport));
        }

        return _coordinatesCalculator.CalculateDistance(new CoordinatesPoint
        {
            Latitude = firstAirportInfo.Latitude,
            Longitude = firstAirportInfo.Longitude
        },
        new CoordinatesPoint
        {
            Latitude = secondAirportInfo.Latitude,
            Longitude = secondAirportInfo.Longitude
        });
    }

    private string CreateErrorMessageString(bool firstAirportFound, string firstAirport, bool secondAirportFound, string secondAirport)
    {
        StringBuilder stringBuilder = new("Airport not found with key(s): ");

        if (firstAirportFound)
        {
            stringBuilder.Append(firstAirport);
        }
        if (secondAirportFound) 
        {
            if (firstAirportFound)
            {
                stringBuilder.Append(", ");
            }
            stringBuilder.Append(secondAirport);
        }

        return stringBuilder.ToString();
    }
}
