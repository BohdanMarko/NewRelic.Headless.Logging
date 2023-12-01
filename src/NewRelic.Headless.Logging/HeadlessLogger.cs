using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;

namespace NewRelic.Headless.Logging;

public sealed class HeadlessLogger : IHeadlessLogger
{
    private readonly HttpClient _client;
    private readonly HeadlessLoggerOptions _options;

    private static AsyncLocal<Stack<(string name, object value)>> Scopes = new AsyncLocal<Stack<(string, object)>>();

    public HeadlessLogger(HttpClient client, IOptions<HeadlessLoggerOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

    public IDisposable BeginScope(string name, object value)
    {
        Scopes.Value ??= new Stack<(string, object)>();
        Scopes.Value.Push((name, value));
        return new DisposableScope();
    }

    private class DisposableScope : IDisposable
    {
        public void Dispose() => Scopes.Value?.Pop();
    }

    public void LogDebug(string message) => Log(LogLevel.Debug, message);
    public async Task LogDebugAsync(string message) => await LogAsync(LogLevel.Debug, message);

    public void LogInfo(string message) => Log(LogLevel.Information, message);
    public async Task LogInfoAsync(string message) => await LogAsync(LogLevel.Information, message);

    public void LogWarning(string message) => Log(LogLevel.Warning, message);
    public async Task LogWarningAsync(string message) => await LogAsync(LogLevel.Warning, message);

    public void LogError(string message) => Log(LogLevel.Error, message);
    public async Task LogErrorAsync(string message) => await LogAsync(LogLevel.Error, message);

    public void LogCritical(string message) => Log(LogLevel.Critical, message);
    public async Task LogCriticalAsync(string message) => await LogAsync(LogLevel.Critical, message);

    public void Log(LogLevel level, string message)
    {
        LogMessage logMessage = BuildLogMessage(level, message);
        HttpRequestMessage request = new()
        {
            Method = HttpMethod.Post,
            Content = new StringContent(JsonConvert.SerializeObject(logMessage), Encoding.UTF8, MediaTypeNames.Application.Json)
        };
        HttpResponseMessage response = _client.Send(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task LogAsync(LogLevel level, string message)
    {
        LogMessage logMessage = BuildLogMessage(level, message);
        StringContent content = new(JsonConvert.SerializeObject(logMessage), Encoding.UTF8, MediaTypeNames.Application.Json);
        HttpResponseMessage response = await _client.PostAsync(string.Empty, content);
        response.EnsureSuccessStatusCode();
    }

    LogMessage BuildLogMessage(LogLevel level, string message) => new()
    {
        Level = level,
        Message = message,
        Entity = new()
        {
            Id = _options.EntityGuid,
            Name = _options.EntityName
        },
        Parameters = Scopes.Value?.ToDictionary(x => x.name, x => x.value)
    };
}