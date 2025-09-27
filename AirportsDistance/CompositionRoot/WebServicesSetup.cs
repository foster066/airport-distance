using AviaExpress.AirportDistance.Core.DistanceAggregate.ExternalService;
using AviaExpress.AirportDistance.Core.Interfaces;
using AviaExpress.AirportDistance.Core.Services;
using AviaExpress.AirportDistance.Infrastructure.Cache;
using AviaExpress.AirportDistance.Infrastructure.ExternalService;
using AviaExpress.AirportDistance.SharedKernel;

namespace AviaExpress.AirportsDistance.Web.CompositionRoot;

public static class WebServicesSetup
{
    public static IServiceCollection AddDistanceWebService(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddStackExchangeRedisCache(o => { });
        serviceCollection.AddHybridCache(options =>
        {
            options.DefaultEntryOptions = new Microsoft.Extensions.Caching.Hybrid.HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromDays(1),
                LocalCacheExpiration = TimeSpan.FromDays(1),
            };
        });

        serviceCollection.Configure<AirportInformationServiceOptions>(
            configuration.GetSection(nameof(AirportInformationServiceOptions)));

        return
        serviceCollection
            .AddScoped<ICache, DistributedSimpleCache>()
            .AddScoped<IAirportInformationService, ContinentAirportInformationService>()
            .AddScoped<ICoordinatesCalculator, MilesCoordinatesCalculator>()
            .AddScoped<IDistanceCalculatorService, DistanceCalculatorService>()
        ;
    }
}