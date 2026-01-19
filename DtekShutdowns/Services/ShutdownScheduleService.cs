using DtekShutdowns.Models;
using DtekShutdowns.Settings;
using Microsoft.Extensions.Options;

namespace DtekShutdowns.Services;

public class ShutdownScheduleService : IShutdownScheduleService
{
    private readonly HashSet<string> _availableGroups;
    private readonly ILogger<ShutdownScheduleService> _logger;
    private readonly IScheduleProvider _scheduleProvider;
    private readonly IScheduleConverter _scheduleParser;

    public ShutdownScheduleService(
        IOptions<GroupsConfig> groupsConfig,
        ILogger<ShutdownScheduleService> logger,
        IScheduleProvider scheduleProvider,
        IScheduleConverter scheduleParser)
    {
        _availableGroups = [.. groupsConfig.Value.AvailableGroups.Select(x => x.Replace(".", ""))];
        _logger = logger;
        _scheduleProvider = scheduleProvider;
        _scheduleParser = scheduleParser;
    }

    public async ValueTask<ShutdownScheduleResult> GetSchedule(string group)
    {
        if (!IsValidGroup(group))
        {
            return new ShutdownScheduleResult
            {
                Status = ResponseStatus.NotFound
            };
        }

        try
        {
            var schedule = await _scheduleProvider.GetSchedule(group);
            var result = _scheduleParser.Parse(schedule).ToList();

            return new ShutdownScheduleResult
            {
                Status = ResponseStatus.Success,
                Result = new ShutdownScheduleResponse(result)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get schedule");
            return new ShutdownScheduleResult
            {
                Status = ResponseStatus.Failed
            };
        }
    }

    private bool IsValidGroup(string group)
    {
        if (string.IsNullOrWhiteSpace(group))
        {
            return false;
        }

        return _availableGroups.Contains(group);
    }
}
