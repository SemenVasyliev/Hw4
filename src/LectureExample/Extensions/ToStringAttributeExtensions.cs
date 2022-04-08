using System.Reflection;
using LectureExample.Attributes;

namespace LectureExample.Extensions;

public static class ToStringAttributeExtensions
{
    /// <summary>
    /// String extension method for <see cref="ToStringAttribute"/>.
    /// </summary>
    /// <param name="target">Target object</param>
    /// <returns>Returns target object string representation</returns>
    public static string? ToStringExt(this object? target)
    {
        if (target == null)
            return null;

        // Try to find ToStringAttribute for target type
        var type = target.GetType();
        var attribute = type.GetCustomAttribute<ToStringAttribute>();

        // Can't find ToStringAttribute or attribute is corrupted - return default implementation
        if (attribute?.Properties == null)
            return target.ToString();

        // Select properties and values
        var properties = attribute.Properties
            .Select(propName => type.GetProperty(propName))
            .Where(prop => prop != null)
            .Select(prop => new
            {
                PropName = prop!.Name,
                PropValue = prop.GetValue(target).ToStringExt()
            })
            .Where(x => x.PropValue != null)
            .Select(x => $"{x.PropName}:{x.PropValue}");

        // Format output
        return $"[{string.Join(',', properties)}]";
    }
}
