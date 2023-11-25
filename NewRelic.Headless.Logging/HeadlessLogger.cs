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

    public HeadlessLogger(HttpClient client, IOptions<HeadlessLoggerOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

    public async Task LogAsync(LogLevel level, string message)
    {
        LogMessage logMessage = new()
        {
            Level = level,
            ServiceName = _options.ServiceName,
            Message = message
        };
        StringContent content = new(JsonConvert.SerializeObject(logMessage), Encoding.UTF8, MediaTypeNames.Application.Json);
        HttpResponseMessage response = await _client.PostAsync(string.Empty, content);
        response.EnsureSuccessStatusCode();
    }
}