using Common;
using FluentAssertions;
using Xunit;

namespace Hw4.Exercise0.Tests;

public class ValidatorApplicationTests
{
    [Theory]
    [InlineData("Tech Academy Student", "18", "50")]
    [InlineData("Tech Academy Student", "200", "200")]
    public void App_Returns_Success(string name, string age, string weight)
    {
        // arrange
        var app = new ValidatorApplication();

        // act
        var exitCode = app.Run(new[] { name, age, weight });

        // assert
        exitCode.Should().Be(ReturnCode.Success);
    }

    [Theory]
    [InlineData(null, "", "")]
    [InlineData("", "", "")]
    [InlineData("", "", "100")]
    [InlineData("", "18", "300")]
    [InlineData("John", "", "")]
    [InlineData("John", "200", "")]
    [InlineData("John", "18", "300")]
    public void App_Returns_InvalidArgs(string name, string age, string weight)
    {
        // arrange
        var app = new ValidatorApplication();

        // act
        var exitCode = app.Run(new[] { name, age, weight });

        // assert
        exitCode.Should().Be(ReturnCode.InvalidArgs);
    }
}
