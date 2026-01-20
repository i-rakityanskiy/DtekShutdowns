using DtekShutdowns.Models;
using HtmlAgilityPack;
using System.Text.Json;

namespace DtekShutdowns.Services;

public class DtekPageParser : IDtekPageParser
{
    private const string ScheduleVariableName = "DisconSchedule.fact";

    public DtekRawSchedule Parse(string htmlPage)
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlPage);
        var scriptNode = htmlDoc.DocumentNode.SelectSingleNode($"//script[contains(text(), '{ScheduleVariableName}')]");
        if (scriptNode == null)
        {
            throw new Exception("Could not find the schedule script node.");
        }
        string scriptContent = scriptNode.InnerText;

        var fact = ExtractJsonFromScript(scriptContent, ScheduleVariableName);
        if (fact == null)
        {
            throw new Exception($"{ScheduleVariableName} not found");
        }

        var (Today, Schedule) = DeserializeDtek(fact);

        return new DtekRawSchedule
        {
            Schedule = Schedule,
            Date = Today
        };
    }

    private (DateTime Today, Dictionary<string, IReadOnlyList<RawScheduleRecord>> Schedule) DeserializeDtek(string jsonString)
    {
        // Deserialize the complex object
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var root = JsonSerializer.Deserialize<DtekRoot>(jsonString, options);
        if (root == null)
        {
            throw new Exception($"Invalid JSON. Could not parse DTEK schedule. jsonString: {jsonString}");
        }

        // Access today's data using the 'today' timestamp provided in the JSON
        string todayKey = root.Today.ToString();
        var groupsToday = root.Data[todayKey];

        // Extract UA date
        var today = CastToUADate(root.Today);

        // Extract all group schedules
        var schedule = new Dictionary<string, IReadOnlyList<RawScheduleRecord>>();

        foreach (var group in groupsToday)
        {
            var groupSchedule = group.Value.Select(kvp => new RawScheduleRecord(
                int.Parse(kvp.Key),
                Enum.Parse<DtekScheduleType>(kvp.Value, true)))
                .ToList();
            var groupKey = group.Key.Replace("GPV", "");
            schedule.Add(groupKey, groupSchedule);
        }

        return (today, schedule);
    }

    private DateTime CastToUADate(long unixTimestamp)
    {
        // 1. Convert to UTC first
        DateTimeOffset utcTime = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);

        // 2. Find the Ukrainian Time Zone
        // Windows ID: "FLE Standard Time"
        // Linux/macOS ID: "Europe/Kyiv"
        string tzId = OperatingSystem.IsWindows() ? "FLE Standard Time" : "Europe/Kyiv";
        TimeZoneInfo uaZone = TimeZoneInfo.FindSystemTimeZoneById(tzId);

        // 3. Convert UTC to Ukrainian Local Time
        DateTime uaTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime.UtcDateTime, uaZone);

        return uaTime;
    }

    private string? ExtractJsonFromScript(string scriptContent, string variableName)
    {
        // 1. Find the start of the variable assignment
        int startIndex = scriptContent.IndexOf(variableName);
        if (startIndex == -1) return null;

        // 2. Find the first opening brace after the variable name
        int braceStart = scriptContent.IndexOf('{', startIndex);
        if (braceStart == -1) return null;

        int bracketCount = 0;
        for (int i = braceStart; i < scriptContent.Length; i++)
        {
            if (scriptContent[i] == '{') bracketCount++;
            else if (scriptContent[i] == '}') bracketCount--;

            // 3. When count returns to 0, we found the end
            if (bracketCount == 0)
            {
                return scriptContent.Substring(braceStart, i - braceStart + 1);
            }
        }

        return null;
    }
}
