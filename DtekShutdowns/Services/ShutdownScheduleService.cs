
using DtekShutdowns.Settings;
using Microsoft.Extensions.Options;

namespace DtekShutdowns.Services;

public class ShutdownScheduleService : IShutdownScheduleService
{
    private readonly HashSet<string> _availableGroups;
    private readonly ILogger<ShutdownScheduleService> _logger;

    public ShutdownScheduleService(IOptions<GroupsConfig> groupsConfig, ILogger<ShutdownScheduleService> logger)
    {
        _availableGroups = [.. groupsConfig.Value.AvailableGroups.Select(x => x.Replace(".", ""))];
        _logger = logger;
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
            await Task.Delay(1000);
            return new ShutdownScheduleResult
            {
                Status = ResponseStatus.Success,
                Result = new Models.ShutdownScheduleResponse()
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
