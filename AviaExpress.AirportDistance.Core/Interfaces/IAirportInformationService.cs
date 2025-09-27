using AviaExpress.AirportDistance.Core.DistanceAggregate.ExternalService;

namespace AviaExpress.AirportDistance.Core.Interfaces;

/// <summary>
/// Interface returning data about an airport.
/// </summary>
public interface IAirportInformationService
{
    /// <summary>
    /// Returns airport information from an underlying service.
    /// </summary>
    /// <param name="iataCode">IATA code of an airport.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Airport info or null if not found.</returns>
    Task<AirportLocation?> GetAirportInfo(string iataCode, CancellationToken cancellationToken);
}
