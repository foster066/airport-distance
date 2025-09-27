using AviaExpress.AirportDistance.Core.Interfaces;
using AviaExpress.AirportsDistance.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace AviaExpress.AirportsDistance.Web.Controllers;

[Route("api/distance")]
[ApiController]
public class DistanceController : ControllerBase
{
    private readonly IDistanceCalculatorService _service;
    private readonly ILogger<DistanceController> _logger;

    public DistanceController(IDistanceCalculatorService service, ILogger<DistanceController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost("airports")]
    public async Task<ActionResult<AirportDistanceResponse>> CalculateDistance(AirportDistanceRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // TODO Use Result or OneOf for distance transfer
            decimal distance = await _service.CalculateDistanceBetweenAirports(request.IataCodeFirst, request.IataCodeSecond, cancellationToken);
            return Ok(new AirportDistanceResponse
            {
                Distance = distance,
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error during airport distance calculation");
            return Problem(e.Message);
        }
    }
}
