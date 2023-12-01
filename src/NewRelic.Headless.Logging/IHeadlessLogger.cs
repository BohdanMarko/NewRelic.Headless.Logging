using Microsoft.Extensions.Logging;

namespace NewRelic.Headless.Logging;

public interface IHeadlessLogger
{
    IDisposable BeginScope(string name, object value);
    void Log(LogLevel level, string message);
    Task LogAsync(LogLevel level, string message);
    void LogCritical(string message);
    Task LogCriticalAsync(string message);
    void LogDebug(string message);
    Task LogDebugAsync(string message);
    void LogError(string message);
    Task LogErrorAsync(string message);
    void LogInfo(string message);
    Task LogInfoAsync(string message);
    void LogWarning(string message);
    Task LogWarningAsync(string message);
}