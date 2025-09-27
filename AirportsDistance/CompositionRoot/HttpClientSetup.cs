using AviaExpress.AirportDistance.Core.DistanceAggregate.ExternalService;
using AviaExpress.AirportDistance.Infrastructure.ExternalService;
using Polly;
using Polly.Extensions.Http;

namespace AviaExpress.AirportsDistance.Web.CompositionRoot;

// TODO Add unit tests for DI
public static class HttpClientSetup
{
    public static IServiceCollection AddHttpClients(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddHttpClient(nameof(ContinentAirportInformationService), c =>
        {
            var options = configuration.GetSection(nameof(AirportInformationServiceOptions)).Get<AirportInformationServiceOptions>();
            c.BaseAddress = new Uri(options.ServiceUrl);
            c.DefaultRequestHeaders.Add("Accept", "application/json");

        }).AddPolicyHandler(GetRetryPolicy())
        .AddPolicyHandler(GetCircuitBreakerPolicy());

        return serviceCollection;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
}