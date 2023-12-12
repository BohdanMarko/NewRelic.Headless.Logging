namespace NewRelic.Headless.Logging.Client;

public sealed class NewRelicClientOptions
{
    public string? ApiKey { get; set; }
    public string? BaseUrl { get; set; }
    public NewRelicRegion Region { get; set; }
}

public enum NewRelicRegion
{
    US,
    EU,
    FedRAMP
}