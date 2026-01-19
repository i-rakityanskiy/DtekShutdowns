namespace DtekShutdowns.Models;

public record ShutdownScheduleResponse
{
    public IEnumerable<object> Data { get; set; }
}
