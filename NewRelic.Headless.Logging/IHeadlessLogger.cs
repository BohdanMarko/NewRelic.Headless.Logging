using Microsoft.Extensions.Logging;

namespace NewRelic.Headless.Logging;

public interface IHeadlessLogger
{
    Task LogAsync(LogLevel level, string message);
}