var builder = DistributedApplication.CreateBuilder(args);
var cache = builder.AddRedis("cache")
                                                .WithRedisCommander()
                                                .WithDataVolume()
                                                .WithPersistence(TimeSpan.FromMinutes(1));

builder.AddProject<Projects.AviaExpress_AirportsDistance_Web>("airportsdistance").WithReference(cache);

builder.AddProject<Projects.AviaExpress_AirportDistance_LoadBalancer>("aviaexpress-airportdistance-loadbalancer");

builder.Build().Run();
