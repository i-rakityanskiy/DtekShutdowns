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

        var result = _sut.Parse(");");
    }
}
