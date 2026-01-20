using DtekShutdowns.Models;
using DtekShutdowns.Services;

namespace DtekShutdowns.Tests;

public class DtekPageParserTests
{
    private readonly IDtekPageParser _sut = new DtekPageParser();

    [Fact]
    public void Parse()
    {
        // Arrange
        var htmlPage = File.ReadAllText(".\\files\\dtek-example.html");

        // Act
        var result = _sut.Parse(htmlPage);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("19.01.2026", result.Date.ToString("dd.MM.yyyy"));
        Assert.Equal("19.01.2026 15:00", result.UpdateDate);
        Assert.Equal(12, result.Schedule.Count);
        Assert.True(result.Schedule.All(item => item.Value.Count == 24));
        Assert.Equal(new RawScheduleRecord(1, DtekScheduleType.first), result.Schedule["5.1"][0]);
        Assert.Equal(new RawScheduleRecord(2, DtekScheduleType.yes), result.Schedule["5.1"][1]);
        Assert.Equal(new RawScheduleRecord(5, DtekScheduleType.no), result.Schedule["5.1"][4]);
        Assert.Equal(new RawScheduleRecord(22, DtekScheduleType.second), result.Schedule["4.2"][21]);
    }
}
