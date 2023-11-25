using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NewRelic.Headless.Logging;

public sealed class LogMessage
{
    [JsonProperty("level")]
    [JsonConverter(typeof(StringEnumConverter))]
    public LogLevel Level { get; set; }

    [JsonProperty("service.name")]
    public string? ServiceName { get; set; }

    [JsonProperty("message")]
    public string? Message { get; set; }
}