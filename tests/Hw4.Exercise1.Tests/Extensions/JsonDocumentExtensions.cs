using System;
using System.Globalization;
using System.Text.Json;

namespace Hw4.Exercise1.Tests.Extensions;

public static class JsonDocumentExtensions
{
    public static bool? GetBoolean(this JsonDocument document, string propertyName)
    {
        return document.GetElement(propertyName)?.GetBoolean();
    }

    public static string GetString(this JsonDocument document, string propertyName)
    {
        return document.GetElement(propertyName)?.GetString();
    }

    public static TimeSpan? GetTimeSpan(this JsonDocument document, string propertyName)
    {
        var duration = document.GetElement(propertyName)?.GetString();
        return (string.IsNullOrWhiteSpace(duration) ||
                !TimeSpan.TryParse(duration, CultureInfo.InvariantCulture, out var timespan))
            ? null
            : timespan;
    }

    public static T[] GetArray<T>(this JsonDocument document, string propertyName)
    {
        var element = document.GetElement(propertyName);

        return element is null || element.Value.ValueKind != JsonValueKind.Array
            ? null
            : element.Value.Deserialize<T[]>();
    }

    private static JsonElement? GetElement(this JsonDocument document, string propertyName)
    {
        return document.RootElement.TryGetProperty(propertyName, out var property) ? property : null;
    }
}
