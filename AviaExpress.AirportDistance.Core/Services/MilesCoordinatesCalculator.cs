using AviaExpress.AirportDistance.Core.DistanceAggregate;
using AviaExpress.AirportDistance.Core.Interfaces;

namespace AviaExpress.AirportDistance.Core.Services;

public class MilesCoordinatesCalculator : ICoordinatesCalculator
{
    public decimal CalculateDistance(CoordinatesPoint firstPoint, CoordinatesPoint secondPoint)
    {
        double distanceInKm = GetDistance(firstPoint.Latitude, firstPoint.Longitude, secondPoint.Latitude,
            secondPoint.Longitude);
        // convert to miles
        return Convert.ToDecimal(Math.Round(distanceInKm / 1.61, 2, MidpointRounding.AwayFromZero));
    }

    private static double GetDistance(decimal latitude1, decimal longitude1, decimal latitude2, decimal longitude2)
    {
        // Convert latitude and longitude to radians
        double rlat1 = Math.PI * (double)latitude1 / 180;
        double rlon1 = Math.PI * (double)longitude1 / 180;
        double rlat2 = Math.PI * (double)latitude2 / 180;
        double rlon2 = Math.PI * (double)longitude2 / 180;

        // Calculate the distance using the Haversine formula
        double dlon = rlon2 - rlon1;
        double dlat = rlat2 - rlat1;
        double a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Pow(Math.Sin(dlon / 2), 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double distance = 6371 * c; // Earth's radius in kilometers

        return distance;
    }
}