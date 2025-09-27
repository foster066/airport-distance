using System.ComponentModel.DataAnnotations;

namespace AviaExpress.AirportsDistance.Web.Models;

public class AirportDistanceRequest
{
    [Required]
    [RegularExpression("^[a-zA-Z]{3}$")]
    [StringLength(3, MinimumLength = 3)]
    public required string IataCodeFirst { get; set; }

    [Required]
    [RegularExpression("^[a-zA-Z]{3}$")]
    [StringLength(3, MinimumLength = 3)]
    public required string IataCodeSecond { get; set; }
}
