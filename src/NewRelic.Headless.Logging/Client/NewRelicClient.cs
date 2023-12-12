using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;

namespace NewRelic.Headless.Logging.Client;

public sealed class NewRelicClient : INewRelicClient
{
    private readonly HttpClient _client;
    
    public NewRelicClient(HttpClient client) => _client = client;

    public void Log(object logEntry)
    {
        ArgumentNullException.ThrowIfNull(logEntry, nameof(logEntry));
        HttpRequestMessage request = new()
        {
            Method = HttpMethod.Post,
            Content = new StringContent(JsonConvert.SerializeObject(logEntry), Encoding.UTF8, MediaTypeNames.Application.Json)
        };
        HttpResponseMessage response = _client.Send(request);
        response.EnsureSuccessStatusCode();
    }
}