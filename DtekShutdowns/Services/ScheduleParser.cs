using DtekShutdowns.Models;

namespace DtekShutdowns.Services;

public class ScheduleParser : IScheduleParser
{
    public IEnumerable<ScheduleRecord> Parse(IEnumerable<RawScheduleRecord> schedule)
    {
        foreach (var (rawPeriod, scheduleType) in schedule)
        {
            var period = rawPeriod - 1;

            switch (scheduleType)
            {
                case DtekScheduleType.yes:
                    yield return new ScheduleRecord($"{period:D2}:00-{period:D2}:59", ShutdownStatus.On);
                    break;

                case DtekScheduleType.no:
                    yield return new ScheduleRecord($"{period:D2}:00-{period:D2}:59", ShutdownStatus.Off);
                    break;

                case DtekScheduleType.first:
                    yield return new ScheduleRecord($"{period:D2}:00-{period:D2}:29", ShutdownStatus.Off);
                    yield return new ScheduleRecord($"{period:D2}:30-{period:D2}:59", ShutdownStatus.On);
                    break;

                case DtekScheduleType.second:
                    yield return new ScheduleRecord($"{period:D2}:00-{period:D2}:29", ShutdownStatus.On);
                    yield return new ScheduleRecord($"{period:D2}:30-{period:D2}:59", ShutdownStatus.Off);
                    break;
            }
        }
    }
}
