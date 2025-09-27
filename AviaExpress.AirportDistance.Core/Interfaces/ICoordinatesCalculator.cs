using AviaExpress.AirportDistance.Core.DistanceAggregate;

namespace AviaExpress.AirportDistance.Core.Interfaces;

/// <summary>
/// Calculator for different types of calculation using coordinates.
/// Returns results in miles. 
/// TODO select results in miles/km
/// </summary>
public interface ICoordinatesCalculator
{
    /// <summary>
    /// Calculates difference between two coordinates.
    /// </summary>
    /// <param name="firstPoint">First point.</param>
    /// <param name="secondPoint">Second point.</param>
    /// <returns>Distance in miles.</returns>
    decimal CalculateDistance(CoordinatesPoint firstPoint, CoordinatesPoint secondPoint);
}