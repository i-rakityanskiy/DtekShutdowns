using DtekShutdowns.Models;
using DtekShutdowns.Settings;
using Microsoft.Extensions.Options;

namespace DtekShutdowns.Services;

public class DtekScheduleProvider : IScheduleProvider
{
    private readonly DtekClientConfig _clientConfig;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IDtekPageParser _dtekPageParser;

    public DtekScheduleProvider(
        IOptions<DtekClientConfig> clientConfig,
        IHttpClientFactory httpClient,
        IDtekPageParser dtekPageParser)
    {
        _clientConfig = clientConfig.Value;
        _httpClientFactory = httpClient;
        _dtekPageParser = dtekPageParser;
    }

    public async ValueTask<IEnumerable<RawScheduleRecord>> GetSchedule(string group)
    {
        // TODO: implement caching

        var result = await GetScheduleFromSite(group);

        return result;
    }

    private async ValueTask<IEnumerable<RawScheduleRecord>> GetScheduleFromSite(string group)
    {
        var htmlPage = await GetShutdownsPage();
        var result = _dtekPageParser.Parse(htmlPage)[group];

        return result;
    }

    private async Task<string> GetShutdownsPage()
    {
        var httpClient = _httpClientFactory.CreateClient();

        var response = await httpClient.GetAsync(_clientConfig.Endpoint);
        response.EnsureSuccessStatusCode();

        var htmlPage = await response.Content.ReadAsStringAsync();
        return htmlPage;
    }
}
