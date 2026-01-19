using DtekShutdowns.Models;

namespace DtekShutdowns.Services;

public interface IScheduleProvider
{
    ValueTask<IEnumerable<RawScheduleRecord>> GetSchedule(string group);
}
