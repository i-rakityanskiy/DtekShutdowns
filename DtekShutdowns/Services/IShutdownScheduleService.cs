namespace DtekShutdowns.Services;

public interface IShutdownScheduleService
{
    ValueTask<ShutdownScheduleResult> GetSchedule(string group);
}
