using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Common;
using FluentAssertions;
using Hw4.Exercise1.Tests.Extensions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Hw4.Exercise1.Tests;

public class PrimesApplicationTests
{
    private const string SettingsFile = "app.settings";
    private const string ResultsFile = "results.json";

    [Fact]
    public void App_Reads_Settings_File()
    {
        // arrange
        var filesProvider = GetFilesProvider();
        filesProvider
            .Exists(Arg.Is<string>(s => s == SettingsFile))
            .Returns(true);
        var app = new PrimesApplication(filesProvider);

        // act
        var exitCode = app.Run();

        // assert
        exitCode.Should().Be(ReturnCode.Success);

        // verify
        filesProvider
            .Received(1)
            .Read(Arg.Is<string>(s => s == SettingsFile));
    }

    [Fact]
    public void App_Settings_File_Return_Error_FileNotFound()
    {
        // arrange
        Stream contentStream = new MemoryStream();
        var filesProvider = GetFilesProvider();
        filesProvider
            .Read(Arg.Is<string>(s => s == SettingsFile))
            .Throws(new FileNotFoundException(SettingsFile));

        filesProvider.Write(
            Arg.Is<string>(s => s == ResultsFile),
            Arg.Do<Stream>(x =>
            {
                x.CopyTo(contentStream);
                contentStream.Seek(0, SeekOrigin.Begin);
            }));

        var app = new PrimesApplication(filesProvider);

        // act
        var exitCode = app.Run();

        // assert
        exitCode.Should().Be(ReturnCode.Error);

        // verify
        filesProvider
            .Received(0)
            .Read(Arg.Is<string>(s => s == SettingsFile));

        // act
        var results = JsonDocument.Parse(contentStream);

        results.GetBoolean("success").Should().BeFalse();
        results.GetString("error").Should().Be("app.settings is missing");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("not a json file")]
    [InlineData("primesFrom=true")]
    [InlineData("primesFrom=[]")]
    [InlineData("primesTo=true")]
    [InlineData("primesTo=[]")]
    public void App_Settings_File_Return_Error_FileCorrupted(string settingsContent)
    {
        // arrange
        var contentStream = new MemoryStream();
        var filesProvider = GetFilesProvider(settingsContent);
        filesProvider
            .Exists(Arg.Is<string>(s => s == SettingsFile))
            .Returns(true);
        filesProvider.Write(
            Arg.Is<string>(s => s == ResultsFile),
            Arg.Do<Stream>(x =>
            {
                x.CopyTo(contentStream);
                contentStream.Seek(0, SeekOrigin.Begin);
            }));

        var app = new PrimesApplication(filesProvider);

        // act
        var exitCode = app.Run();

        // assert
        exitCode.Should().Be(ReturnCode.Error);

        // verify
        filesProvider
            .Received(1)
            .Read(Arg.Is<string>(s => s == SettingsFile));

        // act
        var results = JsonDocument.Parse(contentStream);

        results.GetBoolean("success").Should().BeFalse();
        results.GetString("error").Should().Be("app.settings is corrupted");
    }

    [Fact]
    public void App_Writes_Results_File()
    {
        // arrange
        var contentStream = new MemoryStream();
        var filesProvider = GetFilesProvider();
        filesProvider
            .Exists(Arg.Is<string>(s => s == SettingsFile))
            .Returns(true);
        filesProvider.Write(
            Arg.Is<string>(s => s == ResultsFile),
            Arg.Do<Stream>(x =>
            {
                x.CopyTo(contentStream);
                contentStream.Seek(0, SeekOrigin.Begin);
            }));
        var app = new PrimesApplication(filesProvider);

        // act
        var exitCode = app.Run();

        // assert
        exitCode.Should().Be(ReturnCode.Success);

        // verify
        filesProvider
            .Received(1)
            .Write(Arg.Is<string>(s => s == ResultsFile), Arg.Any<Stream>());

        contentStream.Should().NotBeNull();
    }

    [Theory]
    [InlineData(1, 10, "1-10")]
    [InlineData(2, 7, "2-7")]
    public void App_Writes_Results_File_Correct_Format(int from, int to, string range)
    {
        // arrange
        var contentStream = new MemoryStream();
        var filesProvider = GetFilesProvider(from, to);
        filesProvider
            .Exists(Arg.Is<string>(s => s == SettingsFile))
            .Returns(true);
        filesProvider.Write(
            Arg.Is<string>(s => s == ResultsFile),
            Arg.Do<Stream>(x =>
            {
                x.CopyTo(contentStream);
                contentStream.Seek(0, SeekOrigin.Begin);
            }));
        var app = new PrimesApplication(filesProvider);

        // act
        var exitCode = app.Run();

        // assert
        exitCode.Should().Be(ReturnCode.Success);

        // verify
        filesProvider
            .Received(1)
            .Write(Arg.Is<string>(s => s == ResultsFile), Arg.Any<Stream>());

        contentStream.Should().NotBeNull();

        // act
        var results = JsonDocument.Parse(contentStream);

        results.GetBoolean("success").Should().BeTrue();
        results.GetString("range").Should().Be(range);
        results.GetString("error").Should().BeNull();
        results.GetArray<int>("primes").Should().NotBeNull();

        // assert elapsed time
        var duration = results.GetTimeSpan("duration");
        duration.Should().NotBeNull("\"duration\" property must be present");
        duration!.Value.TotalMilliseconds.Should().BeGreaterThan(0);
    }

    [Theory]
    [InlineData(1, 10, "2,3,5,7")]
    [InlineData(2, 7, "2,3,5,7")]
    [InlineData(-10, 7, "2,3,5,7")]
    [InlineData(2, 5, "2,3,5")]
    [InlineData(2, 3, "2,3")]
    [InlineData(2, 2, "2")]
    [InlineData(2, 1, "")]
    [InlineData(0, 1, "")]
    [InlineData(-1, 0, "")]
    public void App_Writes_Results_File_Correct_Primes(int from, int to, string primes)
    {
        // arrange
        var contentStream = new MemoryStream();
        var filesProvider = GetFilesProvider(from, to);
        filesProvider
            .Exists(Arg.Is<string>(s => s == SettingsFile))
            .Returns(true);
        filesProvider.Write(
            Arg.Is<string>(s => s == ResultsFile),
            Arg.Do<Stream>(x =>
            {
                x.CopyTo(contentStream);
                contentStream.Seek(0, SeekOrigin.Begin);
            }));
        var app = new PrimesApplication(filesProvider);

        // act
        var exitCode = app.Run();

        // assert
        exitCode.Should().Be(ReturnCode.Success);

        // verify
        filesProvider
            .Received(1)
            .Write(Arg.Is<string>(s => s == ResultsFile), Arg.Any<Stream>());

        contentStream.Should().NotBeNull();

        // assert
        var results = JsonDocument.Parse(contentStream);

        results.GetBoolean("success").Should().BeTrue();
        results.GetString("error").Should().BeNull();

        // assert elapsed time
        var duration = results.GetTimeSpan("duration");
        duration.Should().NotBeNull("\"duration\" property must be present");
        duration!.Value.TotalMilliseconds.Should().BeGreaterThan(0);

        // assert calculated primes
        var primesArray = results.GetArray<int>("primes");
        primesArray.Should().NotBeNull();

        var expectedPrimes = string.IsNullOrWhiteSpace(primes)
            ? Array.Empty<int>()
            : primes.Split(",").Select(x => Convert.ToInt32(x, CultureInfo.InvariantCulture)).ToArray();
        primesArray.Should().BeEquivalentTo(expectedPrimes);
    }

    private static IFileSystemProvider GetFilesProvider(int? from = 1, int? to = 10)
    {
        return GetFilesProvider($@"

primesFrom={from}

unexpectedVariable=

primesTo={to}

");
    }

    private static IFileSystemProvider GetFilesProvider(string content)
    {
        var filesProvider = Substitute.For<IFileSystemProvider>();
        filesProvider
            .Read(Arg.Is<string>(s => s == SettingsFile))
            .Returns(new MemoryStream(Encoding.UTF8.GetBytes(content)));

        return filesProvider;
    }
}
