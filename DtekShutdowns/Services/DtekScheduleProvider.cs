using DtekShutdowns.Models;
using DtekShutdowns.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace DtekShutdowns.Services;

public class DtekScheduleProvider : IScheduleProvider
{
    private readonly DtekClientConfig _clientConfig;
    private readonly DtekScheduleProviderConfig _config;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IDtekPageParser _dtekPageParser;
    private readonly IMemoryCache _memoryCache;

    public DtekScheduleProvider(
        IOptions<DtekClientConfig> clientConfig,
        IOptions<DtekScheduleProviderConfig> config,
        IHttpClientFactory httpClient,
        IDtekPageParser dtekPageParser,
        IMemoryCache memoryCache)
    {
        _clientConfig = clientConfig.Value;
        _config = config.Value;
        _httpClientFactory = httpClient;
        _dtekPageParser = dtekPageParser;
        _memoryCache = memoryCache;
    }

    public async ValueTask<ScheduleProviderResponse> GetSchedule(string group)
    {
        var schedules = await GetAllSchedules();
        var result = schedules.Schedule[group];

        return new ScheduleProviderResponse(result, schedules.Date, schedules.UpdateDate);
    }

    private async Task<DtekRawSchedule> GetAllSchedules()
    {
        var cacheKey = $"DtekSchedule_{DateTime.Now:yyyyMMdd}";
        _memoryCache.TryGetValue<DtekRawSchedule>(cacheKey, out var result);
        if (result != null)
        {
            return result;
        }

        result = await GetScheduleFromSite();
        _memoryCache.Set(
            cacheKey,
            result,
            new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_config.CacheTtlMinutes),
                Priority = CacheItemPriority.High
            });

        return result;
    }

    private async Task<DtekRawSchedule> GetScheduleFromSite()
    {
        var htmlPage = await GetShutdownsPage();
        var schedule = _dtekPageParser.Parse(htmlPage);
        return schedule;
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
