namespace DtekShutdowns.Models;

public class DtekRawSchedule
{
    public required IReadOnlyDictionary<string, IReadOnlyList<RawScheduleRecord>> Schedule { get; init; }
    public required DateTime Date { get; init; }
    public string? UpdateDate { get; init; }
}
