using DtekShutdowns.Models;

namespace DtekShutdowns.Services;

public interface IScheduleParser
{
    IEnumerable<ScheduleRecord> Parse(IEnumerable<RawScheduleRecord> schedule);
}
