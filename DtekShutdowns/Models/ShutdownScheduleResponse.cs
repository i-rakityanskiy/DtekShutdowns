using System.Text.Json.Serialization;

namespace DtekShutdowns.Models;

public record ShutdownScheduleResponse(IEnumerable<ScheduleRecord> Schedule, string Date, string group);

public record ScheduleRecord(string Period, ShutdownStatus Status);

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ShutdownStatus
{
    // ✅
    On,
    // ❌
    Off,
    Unknown
}