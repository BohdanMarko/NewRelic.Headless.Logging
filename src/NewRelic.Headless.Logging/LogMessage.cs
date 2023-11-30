using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NewRelic.Headless.Logging;

public sealed class LogMessage
{
    [JsonProperty("level")]
    [JsonConverter(typeof(StringEnumConverter))]
    public LogLevel Level { get; set; }

    [JsonProperty("entity")]
    public Entity? Entity { get; set; }

    [JsonProperty("message")]
    public string? Message { get; set; }
}

public sealed class  Entity
{
    [JsonProperty("guid")]
    public string? Id { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }
}