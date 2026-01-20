using DtekShutdowns.Models;

namespace DtekShutdowns.Services;

public record ScheduleProviderResponse(IEnumerable<RawScheduleRecord> Schedule, DateTime Date, string? UpdateDate);
