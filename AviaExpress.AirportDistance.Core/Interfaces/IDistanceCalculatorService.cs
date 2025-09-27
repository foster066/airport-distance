namespace AviaExpress.AirportDistance.Core.Interfaces;

/// <summary>
/// Interface describing service for distance calculator.
/// </summary>
public interface IDistanceCalculatorService
{
    /// <summary>
    /// Calculates a distance between two airports in miles.
    /// </summary>
    /// <param name="firstAirport">Iata code for a one of airport.</param>
    /// <param name="secondAirport">Iata code for another airport.</param>
    /// <returns>Distance between two airports in miles.</returns>
    /// <param name="cancellationToken"></param>
    Task<decimal> CalculateDistanceBetweenAirports(string firstAirport, string secondAirport, CancellationToken cancellationToken);
}
