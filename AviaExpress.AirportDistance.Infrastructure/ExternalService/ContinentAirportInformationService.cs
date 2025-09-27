using System.Net.Http.Json;
using AviaExpress.AirportDistance.Core.DistanceAggregate.ExternalService;
using AviaExpress.AirportDistance.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace AviaExpress.AirportDistance.Infrastructure.ExternalService;

public class ContinentAirportInformationService : IAirportInformationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ContinentAirportInformationService> _logger;

    public ContinentAirportInformationService(IHttpClientFactory httpClientFactory, ILogger<ContinentAirportInformationService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }
    public async Task<AirportLocation?> GetAirportInfo(string iataCode, CancellationToken cancellationToken)
    {
        try
        {
            HttpClient httpClient = _httpClientFactory.CreateClient(nameof(ContinentAirportInformationService));
            HttpResponseMessage response = await httpClient.GetAsync(iataCode, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                AirportInfo? airportInfo = await response.Content.ReadFromJsonAsync<AirportInfo>(cancellationToken);

                return airportInfo?.Location is not null ? new AirportLocation
                {
                    Latitude = airportInfo.Location!.Latitude,
                    Longitude = airportInfo.Location!.Longitude,
                } : null;   
            }

            _logger.LogError($"Not successful status code from airport information service for '{iataCode}'. Status code: {response.StatusCode}, message: {response.ReasonPhrase}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error receiving airport information from service for '{iataCode}'");
            throw;
        }

        return null;
    }
}
