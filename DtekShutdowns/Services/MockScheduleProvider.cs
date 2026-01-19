using System.Text.Json;
using DtekShutdowns.Models;

namespace DtekShutdowns.Services;

public class MockScheduleProvider : IScheduleProvider
{
    public async ValueTask<IEnumerable<RawScheduleRecord>> GetSchedule(string group)
    {
        return GetMockSchedule();
    }

    private List<RawScheduleRecord> GetMockSchedule()
    {
        string jsonInput = """
{
    "1": "no", "2": "no", "3": "no", "4": "no", "5": "yes", "6": "yes",
    "7": "yes", "8": "second", "9": "no", "10": "no", "11": "no", "12": "no",
    "13": "no", "14": "no", "15": "first", "16": "yes", "17": "yes", "18": "yes",
    "19": "no", "20": "no", "21": "no", "22": "no", "23": "yes", "24": "yes"
}
""";

        // 2. Parse into a dictionary
        // 2. Parse into a dictionary
        var rawData = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonInput);

        // 3. Convert to List<RawScheduleRecord>
        List<RawScheduleRecord> schedule = rawData.Select(kvp => new RawScheduleRecord(
            int.Parse(kvp.Key),
            Enum.Parse<DtekScheduleType>(kvp.Value, ignoreCase: true)
        )).OrderBy(r => r.Period).ToList();

        return schedule;
    }
}
