using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace Hw4.Exercise0.Tests;

public class PersonValidatorTests
{
    [Fact]
    public void Returns_Error_When_Null()
    {
        // arrange
        var person = GetPerson();

        // act
        var result = PersonValidator.Validate(person);

        // assert
        result.IsValid.Should().BeFalse();
        result.ErrorMessage.Should().Be("Person can't be null");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Returns_Error_When_Name_Invalid(string name)
    {
        // arrange
        var person = GetPerson(new
        {
            Name = name,
            Age = 28,
            Weight = 50
        });

        // act
        var result = PersonValidator.Validate(person);

        // assert
        result.IsValid.Should().BeFalse();
        result.ErrorMessage.Should().Be("Person Name is required", $"'{name}' considered invalid");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(16)]
    [InlineData(201)]
    public void Returns_Error_When_Age_Invalid(int? age)
    {
        // arrange
        var person = GetPerson(new
        {
            Name = "Student",
            Age = age,
            Weight = 50
        });

        // act
        var result = PersonValidator.Validate(person);

        // assert
        result.IsValid.Should().BeFalse();
        result.ErrorMessage.Should().Be("Person Age is out of range", $"{age} considered invalid");
    }

    [Theory]
    [InlineData(-1d)]
    [InlineData(0d)]
    [InlineData(201d)]
    [InlineData(300d)]
    public void Returns_Error_When_Weight_Invalid(double? weight)
    {
        // arrange
        var person = GetPerson(new
        {
            Name = "Student",
            Age = 18,
            Weight = weight
        });

        // act
        var result = PersonValidator.Validate(person);

        // assert
        result.IsValid.Should().BeFalse();
        result.ErrorMessage.Should().Be("Person Weight is out of range", $"{weight} considered invalid");
    }

    private static Person? GetPerson(object? source = null)
    {
        if (source is null)
            return null;

        var json = JsonSerializer.Serialize(source);
        try
        {
            return JsonSerializer.Deserialize<Person>(json);
        }
        catch
        {
            return null;
        }
    }
}
