using DtekShutdowns.Models;

namespace DtekShutdowns.Services;

public interface IScheduleConverter
{
    IEnumerable<ScheduleRecord> Parse(ScheduleProviderResponse schedule);
}
