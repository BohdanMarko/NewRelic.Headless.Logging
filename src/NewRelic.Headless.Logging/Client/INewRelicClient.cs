namespace NewRelic.Headless.Logging.Client;

public interface INewRelicClient
{
    void Log(object logEntry);
}