namespace NewRelic.Headless.Logging.Providers;

public sealed class NewRelicLoggerConfiguration
{
    public bool Enabled { get; set; }
    public string EntityGuid { get; set; }
    public string EntityName { get; set; }
}
