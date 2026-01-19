namespace DtekShutdowns.Models;

public record RawScheduleRecord(int Period, DtekScheduleType ScheduleType);

public enum DtekScheduleType
{
    /// <summary>
    /// lights off
    /// </summary>
    no,
    /// <summary>
    /// lights on
    /// </summary>
    yes,
    /// <summary>
    /// off first half of an hour
    /// </summary>
    first,
    /// <summary>
    /// off second half of an hour
    /// </summary>
    second
}
