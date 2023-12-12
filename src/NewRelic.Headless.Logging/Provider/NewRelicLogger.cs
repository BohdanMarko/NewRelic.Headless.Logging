using Microsoft.Extensions.Logging;
using NewRelic.Headless.Logging.Client;
using NewRelic.Headless.Logging.Provider;

namespace NewRelic.Headless.Logging.Providers;

public sealed class NewRelicLogger : ILogger
{
    private readonly string _name;
    private readonly Func<NewRelicLoggerConfiguration> _getCurrentConfig;
    private readonly Func<INewRelicClient> _getClient;

    private static AsyncLocal<Stack<ScopeEntry>> Scopes = new AsyncLocal<Stack<ScopeEntry>>();

    public NewRelicLogger(string name, Func<NewRelicLoggerConfiguration> getCurrentConfig, Func<INewRelicClient> getClient)
    {
        _name = name;
        _getCurrentConfig = getCurrentConfig;
        _getClient = getClient;
    }

    public IDisposable BeginScope<TState>(TState state) where TState : notnull
    {
        Scopes.Value ??= new Stack<ScopeEntry>();
        ScopeEntry? scopeEntry = state as ScopeEntry;
        Scopes.Value.Push(scopeEntry!);
        return new DisposableScope();
    }  

    public bool IsEnabled(LogLevel logLevel) => _getCurrentConfig().Enabled;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        var config = _getCurrentConfig();

        LogEntry logEntry = new() 
        {
            Level = logLevel,
            Name = _name,
            Message = formatter(state, exception),
            Scopes = Scopes.Value?.Select(s => new Scope { Name = s.Name, Value = s.Value }).ToList(),
            Entity = new Entity
            {
                Id = config.EntityGuid,
                Name = config.EntityName
            }
        };

        _getClient().Log(logEntry);
    }

    sealed class DisposableScope : IDisposable
    {
        public void Dispose() => Scopes.Value?.Pop();
    }
}
