using Microsoft.Extensions.Logging;

namespace NewRelic.Headless.Logging.Client;

public interface INewRelicClient
{
    void Log(object logEntry);
}