namespace NewRelic.Headless.Logging;

public sealed class HeadlessLoggerOptions
{
    public string? ApiKey { get; set; }
    public string? EntityGuid { get; set; }
    public string? EntityName { get; set; }
    public string? BaseUrl { get; set; }
    public NewRelicRegion Region { get; set; }
}

public enum NewRelicRegion
{
    US,
    EU,
    FedRAMP
}