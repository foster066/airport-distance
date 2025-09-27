using System.Text.Json.Serialization;

namespace AviaExpress.AirportDistance.Infrastructure.ExternalService;

/// <summary>
/// Class containing full information about an airport for services.
/// </summary>
public class AirportInfo
{
    [JsonPropertyName("iata")]
    public string? Iata { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("city_iata")]
    public string? City_iata { get; set; }

    [JsonPropertyName("icao")]
    public string? Icao { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("country_iata")]
    public string? Country_iata { get; set; }

    [JsonPropertyName("location")]
    public Location? Location { get; set; }

    [JsonPropertyName("rating")]
    public int Rating { get; set; }

    [JsonPropertyName("hubs")]
    public int Hubs { get; set; }

    [JsonPropertyName("timezone_region_name")]
    public string? TimezoneRegionName { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}

public class Location
{
    [JsonPropertyName("lon")]
    public decimal Longitude { get; set; }

    [JsonPropertyName("lat")]
    public decimal Latitude { get; set; }
}