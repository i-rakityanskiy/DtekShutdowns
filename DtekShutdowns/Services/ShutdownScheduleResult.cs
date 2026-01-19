using DtekShutdowns.Models;

namespace DtekShutdowns.Services;

public record ShutdownScheduleResult
{
    public required ResponseStatus Status { init; get; }

    public ShutdownScheduleResponse? Result { init; get; }
}
