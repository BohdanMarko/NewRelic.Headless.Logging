using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NewRelic.Headless.Logging.Client;
using System.Collections.Concurrent;
using System.Runtime.Versioning;

namespace NewRelic.Headless.Logging.Providers;


[UnsupportedOSPlatform("browser")]
[ProviderAlias("NewRelicLogger")]
public sealed class NewRelicLoggerProvider : ILoggerProvider
{
    private readonly IDisposable? _onChangeToken;
    private NewRelicLoggerConfiguration _currentConfig;
    private readonly ConcurrentDictionary<string, NewRelicLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);
    private readonly IServiceProvider _serviceProvider;

    public NewRelicLoggerProvider(IOptionsMonitor<NewRelicLoggerConfiguration> config, IServiceProvider serviceProvider)
    {
        _currentConfig = config.CurrentValue;
        _onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
        _serviceProvider = serviceProvider;
    }

    public ILogger CreateLogger(string categoryName) 
        => _loggers.GetOrAdd(categoryName, name => new NewRelicLogger(name, GetCurrentConfig, GetNewRelicClient));

    private NewRelicLoggerConfiguration GetCurrentConfig() 
        => _currentConfig;

    private INewRelicClient GetNewRelicClient() 
        => _serviceProvider.GetRequiredService<INewRelicClient>();

    public void Dispose()
    {
        _loggers.Clear();
        _onChangeToken?.Dispose();
    }
}
