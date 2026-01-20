namespace DtekShutdowns.Models;

public class DtekRoot
{
    // The main data container: Date -> Group -> Hour -> Status
    public Dictionary<string, Dictionary<string, Dictionary<string, string>>> Data { get; set; }
    public string Update { get; set; }
    public long Today { get; set; }
}