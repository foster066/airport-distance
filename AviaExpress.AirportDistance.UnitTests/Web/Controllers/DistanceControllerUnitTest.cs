using AviaExpress.AirportDistance.Core.Interfaces;
using AviaExpress.AirportsDistance.Web.Controllers;
using AviaExpress.AirportsDistance.Web.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AviaExpress.AirportDistance.UnitTests.Web.Controllers;

public class DistanceControllerUnitTest
{
    private readonly IDistanceCalculatorService _service;
    private readonly ILogger<DistanceController> _logger;
    private readonly DistanceController _controller;

    public DistanceControllerUnitTest()
    {
        _logger = A.Fake<ILogger<DistanceController>>();
        _service = A.Fake<IDistanceCalculatorService>();

        _controller = new(_service, _logger);
    }

    [Fact]
    public async Task CalculateDistance_NormalCall_ResponseReturned()
    {
        // Arrange
        AirportDistanceRequest request = new()
        {
            IataCodeFirst = "XXX",
            IataCodeSecond = "AAA"
        };
        const decimal expectedDistance = 123.45M;
        A.CallTo(() => _service.CalculateDistanceBetweenAirports(request.IataCodeFirst, request.IataCodeSecond,
            CancellationToken.None))
        .Returns(Task.FromResult(expectedDistance));

        // Act
        var response = await _controller.CalculateDistance(request, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(response.Result);
        Assert.NotNull(okResult.Value);
        var decimalResponse = Assert.IsType<AirportDistanceResponse>(okResult.Value);
        Assert.Equal(expectedDistance, decimalResponse.Distance);
    }
}