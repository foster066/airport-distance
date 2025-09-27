using AviaExpress.AirportDistance.Core.DistanceAggregate.ExternalService;
using FakeItEasy;
using System.Net;
using AviaExpress.AirportDistance.Core.Interfaces;
using AviaExpress.AirportDistance.Infrastructure.ExternalService;
using Microsoft.Extensions.Logging;

namespace AviaExpress.AirportDistance.UnitTests.Core.Services.ExternalServices;

public class ContinentAirportInformationServiceUnitTest
{
    private readonly HttpClient _httpClient;
    private readonly HttpMessageHandler _httpHandler;
    private readonly IAirportInformationService _service;

    public ContinentAirportInformationServiceUnitTest()
    {
        _httpHandler = A.Fake<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpHandler);
        _httpClient.BaseAddress = new Uri("http://localhost/");
        
        var httpClientFactory = A.Fake<IHttpClientFactory>();
        A.CallTo(() => httpClientFactory.CreateClient(nameof(ContinentAirportInformationService))).Returns(_httpClient);

        _service = new ContinentAirportInformationService(httpClientFactory, A.Fake<ILogger<ContinentAirportInformationService>>());
    }

    [Fact]
    public async Task GetAirportInfo_NormalExecution_ReturnsResult()
    {
        // Arrange
        decimal latitudeExpected = 52.309069M;
        decimal longitudeExpected = 4.763385M;
        const string airportCode = "XXX";
        const string expectedAnswer = """
                                      {
                                        "location": {
                                          "lon": 4.763385,
                                          "lat": 52.309069
                                        }
                                      }
                                      """;
        SetupResponse(expectedAnswer, airportCode, HttpStatusCode.OK);

        // Act
        AirportLocation? location = await _service.GetAirportInfo(airportCode, CancellationToken.None);

        // Assert
        Assert.NotNull(location);
        Assert.Equal(latitudeExpected, location.Latitude);
        Assert.Equal(longitudeExpected, location.Longitude);
    }

    [Fact]
    public async Task GetAirportInfo_NotSuccessfulCode_ReturnsNull()
    {
        // Arrange
        const string airportCode = "XXX";
        SetupResponse(string.Empty, airportCode, HttpStatusCode.InternalServerError);

        // Act
        AirportLocation? location = await _service.GetAirportInfo(airportCode, CancellationToken.None);

        // Assert
        Assert.Null(location);
    }

    private void SetupResponse(string expectedAnswer, string airportCode, HttpStatusCode code)
    {
        var response = new HttpResponseMessage
        {
            StatusCode = code,
            Content = new StringContent(expectedAnswer)
        };


        A.CallTo(_httpHandler).Where(x => x.Method.Name == "SendAsync" &&
                                          (x.Arguments[0] as HttpRequestMessage).RequestUri ==
                                          new Uri(_httpClient.BaseAddress + airportCode))
            .WithReturnType<Task<HttpResponseMessage>>()
            .Returns(Task.FromResult(response));
    }
}