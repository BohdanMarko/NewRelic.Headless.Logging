using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NewRelic.Headless.Logging.Client;
using NewRelic.Headless.Logging.Providers;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace NewRelic.Headless.Logging;

public static class DependencyInjectionExtensions
{
    public static ILoggingBuilder AddNewRelicLogger(this ILoggingBuilder builder)
    {
        builder.AddConfiguration();
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, NewRelicLoggerProvider>());
        LoggerProviderOptions.RegisterProviderOptions<NewRelicLoggerConfiguration, NewRelicLoggerProvider>(builder.Services);
        return builder;
    }

    public static ILoggingBuilder AddNewRelicLogger(this ILoggingBuilder builder, Action<NewRelicLoggerConfiguration> configure)
    {
        builder.AddNewRelicLogger();
        builder.Services.Configure(configure);
        return builder;
    }

    public static IServiceCollection AddNewRelicClient(this IServiceCollection services, Action<NewRelicClientOptions> options)
    {
        services.Configure(options);
        services.AddHttpClient<INewRelicClient, NewRelicClient>((serviceProvider, client) =>
        {
            NewRelicClientOptions optionsConfig = serviceProvider.GetRequiredService<IOptions<NewRelicClientOptions>>().Value;
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
