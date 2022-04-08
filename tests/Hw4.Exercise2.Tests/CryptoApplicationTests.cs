using System.Text;
using Common;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Hw4.Exercise2.Tests;

public class CryptoApplicationTests
{
    [Fact]
    public void App_Reads_Source_File()
    {
        // arrange
        var filesProvider = Substitute.For<IFileSystemProvider>();
        filesProvider
            .Exists(Arg.Is<string>(s => s == "input.txt"))
            .Returns(true);
        filesProvider
            .Read(Arg.Is<string>(s => s == "input.txt"))
            .Returns(new MemoryStream(Encoding.UTF8.GetBytes("")));

        var app = new CryptoApplication(filesProvider);

        // act
        var exitCode = app.Run(new[] { "input.txt" });

        // assert
        exitCode.Should().Be(ReturnCode.Success);

        // verify
        filesProvider
            .Received(1)
            .Read(Arg.Is<string>(s => s == "input.txt"));
    }

    [Fact]
    public void App_Reads_Source_File_FileNotFound()
    {
        // arrange
        var filesProvider = Substitute.For<IFileSystemProvider>();
        filesProvider
            .Exists(Arg.Is<string>(s => s == "input.txt"))
            .Returns(false);

        var app = new CryptoApplication(filesProvider);

        // act
        var exitCode = app.Run(new[] { "input.txt" });

        // assert
        exitCode.Should().Be(ReturnCode.Error);

        // verify
        filesProvider
            .DidNotReceive()
            .Read(Arg.Is<string>(s => s == "input.txt"));
    }

    [Theory]
    [InlineData("aaa", "bbb")]
    [InlineData("AAA", "BBB")]
    [InlineData("abc", "bcd")]
    [InlineData("Abc", "Bcd")]
    [InlineData("dEf", "eFg")]
    [InlineData("xyZ", "yz0")]
    [InlineData("Hello Tech Academy!", "Ifmmp Ufdi Bdbefnz!")]
    [InlineData("Hello \nTech \nAcademy!", "Ifmmp \nUfdi \nBdbefnz!")]
    public void App_Encodes_Input_File_GivenOffset(string sourceText, string expectedText)
    {
        // arrange
        Stream? contentStream = null;
        var filesProvider = Substitute.For<IFileSystemProvider>();
        filesProvider
            .Exists(Arg.Is<string>(s => s == "input.txt"))
            .Returns(true);

        filesProvider
            .Read(Arg.Is<string>(s => s == "input.txt"))
            .Returns(new MemoryStream(Encoding.UTF8.GetBytes(sourceText)));

        filesProvider.Write(
            Arg.Is<string>(s => s == "input.txt.enc"),
            Arg.Do<Stream>(x =>
            {
                contentStream = x;
                contentStream.Seek(0, SeekOrigin.Begin);
            }));

        var app = new CryptoApplication(filesProvider);

        // act
        var exitCode = app.Run(new[] { "input.txt", "1" });

        // assert
        exitCode.Should().Be(ReturnCode.Success);

        // verify
        filesProvider
            .Received(1)
            .Write(Arg.Is<string>(s => s == "input.txt.enc"), Arg.Any<Stream>());

        contentStream.Should().NotBeNull();

        var writtenText = new StreamReader(contentStream!).ReadToEnd();

        writtenText.Should().Be(expectedText);
    }

    [Theory]
    [InlineData("000", "333")]
    [InlineData("abc", "def")]
    [InlineData("def", "ghi")]
    [InlineData("xyz", "012")]
    [InlineData("Hello Tech Academy!", "Khoor Whfk Dfdghp1!")]
    public void App_Encodes_Input_File(string sourceText, string expectedText)
    {
        // arrange
        Stream? contentStream = null;
        var filesProvider = Substitute.For<IFileSystemProvider>();
        filesProvider
            .Exists(Arg.Is<string>(s => s == "input.txt"))
            .Returns(true);
        filesProvider
            .Read(Arg.Is<string>(s => s == "input.txt"))
            .Returns(new MemoryStream(Encoding.UTF8.GetBytes(sourceText)));

        filesProvider.Write(
            Arg.Is<string>(s => s == "input.txt.enc"),
            Arg.Do<Stream>(x =>
            {
                contentStream = x;
                contentStream.Seek(0, SeekOrigin.Begin);
            }));

        var app = new CryptoApplication(filesProvider);

        // act
        var exitCode = app.Run(new[] { "input.txt" });

        // assert
        exitCode.Should().Be(ReturnCode.Success);

        // verify
        filesProvider
            .Received(1)
            .Write(Arg.Is<string>(s => s == "input.txt.enc"), Arg.Any<Stream>());

        contentStream.Should().NotBeNull();

        var writtenText = new StreamReader(contentStream!).ReadToEnd();

        writtenText.Should().Be(expectedText);
    }

    [Theory]
    [InlineData("def", "abc")]
    [InlineData("ghi", "def")]
    [InlineData("abc", "789")]
    [InlineData("Khoor Whfk Dfdghp1!", "Hello Tech Academy!")]
    public void App_Decodes_Input_File(string sourceText, string expectedText)
    {
        // arrange
        Stream? contentStream = null;
        var filesProvider = Substitute.For<IFileSystemProvider>();
        filesProvider
            .Exists(Arg.Is<string>(s => s == "input.txt"))
            .Returns(true);
        filesProvider
            .Read(Arg.Is<string>(s => s == "input.txt"))
            .Returns(new MemoryStream(Encoding.UTF8.GetBytes(sourceText)));

        filesProvider.Write(
            Arg.Is<string>(s => s == "input.txt.dec"),
            Arg.Do<Stream>(x =>
            {
                contentStream = x;
                contentStream.Seek(0, SeekOrigin.Begin);
            }));

        var app = new CryptoApplication(filesProvider);

        // act
        var exitCode = app.Run(new[] { "input.txt", "3", "dec" });

        // assert
        exitCode.Should().Be(ReturnCode.Success);

        // verify
        filesProvider
            .Received(1)
            .Write(Arg.Is<string>(s => s == "input.txt.dec"), Arg.Any<Stream>());

        contentStream.Should().NotBeNull();

        var writtenText = new StreamReader(contentStream!).ReadToEnd();

        writtenText.Should().Be(expectedText);
    }
}
