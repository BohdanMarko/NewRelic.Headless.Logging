using Microsoft.Extensions.Logging;
using NewRelic.Headless.Logging.Providers;

namespace NewRelic.Headless.Logging.Provider;

public static class LoggerExtensions
{
    public static IDisposable? BeginScope(this ILogger logger, string scopeName, object scopeValue)
    {
        ArgumentNullException.ThrowIfNull(logger);
        return logger.BeginScope(new ScopeEntry(scopeName, scopeValue));
    }
}
