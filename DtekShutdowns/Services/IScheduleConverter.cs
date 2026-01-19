using DtekShutdowns.Models;

namespace DtekShutdowns.Services;

public interface IScheduleConverter
{
    IEnumerable<ScheduleRecord> Parse(IEnumerable<RawScheduleRecord> schedule);
}
