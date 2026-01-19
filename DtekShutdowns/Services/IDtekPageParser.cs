using DtekShutdowns.Models;

namespace DtekShutdowns.Services;

public interface IDtekPageParser
{
    DtekRawSchedule Parse(string htmlPage);
}
