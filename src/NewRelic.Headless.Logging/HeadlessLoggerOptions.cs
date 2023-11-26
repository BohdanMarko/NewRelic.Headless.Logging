namespace NewRelic.Headless.Logging;

public sealed class HeadlessLoggerOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public NewRelicRegion Region { get; set; }
}

public enum NewRelicRegion
{
    US,
    EU,
    FedRAMP
}