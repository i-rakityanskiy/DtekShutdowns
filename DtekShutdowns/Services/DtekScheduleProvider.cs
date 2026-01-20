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

    public async ValueTask<ScheduleProviderResponse> GetSchedule(string group)
    {
        // TODO: implement caching

        var result = await GetScheduleFromSite(group);

        return result;
    }

    private async ValueTask<ScheduleProviderResponse> GetScheduleFromSite(string group)
    {
        var htmlPage = await GetShutdownsPage();
        var schedules = _dtekPageParser.Parse(htmlPage);
        var result = schedules.Schedule[group];

        return new ScheduleProviderResponse(result, schedules.Date);
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
