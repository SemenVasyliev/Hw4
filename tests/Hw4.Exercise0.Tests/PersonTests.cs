using System.Reflection;
using FluentAssertions;
using Xunit;

namespace Hw4.Exercise0.Tests;

public class PersonTests
{
    private readonly PropertyInfo[] _properties = typeof(Person).GetProperties();

    [Fact]
    public void Ensure_Properties_Defined()
    {
        // act & assert
        EnsurePropertyDefined("Name");
        EnsurePropertyDefined("Age");
        EnsurePropertyDefined("Weight");
    }

    [Fact]
    public void Ensure_Properties_Has_Attributes()
    {
        // act & assert
        EnsureHasAttributes("Name");
        EnsureHasAttributes("Age");
        EnsureHasAttributes("Weight");
    }

    private void EnsureHasAttributes(string propertyName)
    {
        var property = EnsurePropertyDefined(propertyName);
        var propertyAttributes = property.GetCustomAttributes();

        propertyAttributes
            .Should()
            .ContainSingle(x => x != null,
                $"property \"{propertyName}\" must be marked with validation attribute"
            );
    }

    private PropertyInfo EnsurePropertyDefined(string propertyName)
    {
        return _properties
            .Should()
            .ContainSingle(x => x.Name == propertyName,
                $"property \"{propertyName}\" must be defined"
            ).Subject;
    }
}
