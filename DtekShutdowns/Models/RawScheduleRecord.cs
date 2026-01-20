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
    second,
    maybe,
    mfirst,
    msecond,
}

/*
  "time_type": {
    "yes": "Світло є",
    "maybe": "Можливо відключення",
    "no": "Світла немає",
    "first": "Світла не буде перші 30 хв.",
    "second": "Світла не буде другі 30 хв",
    "mfirst": "Світла можливо не буде перші 30 хв.",
    "msecond": "Світла можливо не буде другі 30 хв"
  }
*/