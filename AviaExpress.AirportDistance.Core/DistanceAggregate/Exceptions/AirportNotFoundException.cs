namespace AviaExpress.AirportDistance.Core.DistanceAggregate.Exceptions;

public class AirportNotFoundException : Exception
{
    public AirportNotFoundException(string message) : base(message)
    {
            
    }
}