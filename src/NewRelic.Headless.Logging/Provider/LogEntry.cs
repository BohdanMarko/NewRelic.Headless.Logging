using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NewRelic.Headless.Logging.Provider;

public sealed class LogEntry
{
    [JsonProperty("level")]
    [JsonConverter(typeof(StringEnumConverter))]
    public LogLevel Level { get; set; }

    [JsonProperty("logger.name")]
    public string? Name { get; set; }

    [JsonProperty("entity")]
    public Entity? Entity { get; set; }

    [JsonProperty("message")]
    public string? Message { get; set; }

    [JsonProperty("scopes")]
    public List<Scope> Scopes { get; set; }
}

public sealed class Entity
{
    [JsonProperty("guid")]
    public string? Id { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }
}

public sealed class Scope
{
    [JsonProperty("scope.name")]
    public string? Name { get; set; }

    [JsonProperty("scope.value")]
    public object? Value { get; set; }
}