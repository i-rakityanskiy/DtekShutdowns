using DtekShutdowns.Models;

namespace DtekShutdowns.Services;

public interface IScheduleProvider
{
    ValueTask<ScheduleProviderResponse> GetSchedule(string group);
}
