using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace NewRelic.Headless.Logging;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNewRelicHeadlessLogger(this IServiceCollection services, Action<HeadlessLoggerOptions> options)
    {
        services.Configure(options);

        services.AddHttpClient<IHeadlessLogger, HeadlessLogger>((serviceProvider, client) =>
        {
            HeadlessLoggerOptions optionsConfig = serviceProvider.GetRequiredService<IOptions<HeadlessLoggerOptions>>().Value;
            client.BaseAddress = new Uri(optionsConfig.BaseUrl ?? ResolveBaseAddress(optionsConfig.Region));
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Api-Key", optionsConfig.ApiKey);
        });

        return services;
    }

    static string ResolveBaseAddress(NewRelicRegion region) => region switch
    {
        NewRelicRegion.US => "https://log-api.newrelic.com/log/v1",
        NewRelicRegion.EU => "https://log-api.eu.newrelic.com/log/v1",
        NewRelicRegion.FedRAMP => "https://gov-log-api.newrelic.com/log/v1",
        _ => throw new ArgumentException($"Unknown region: {region}", nameof(region))
    };
}
