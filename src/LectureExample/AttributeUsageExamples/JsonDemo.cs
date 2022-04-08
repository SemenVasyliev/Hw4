using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace LectureExample.AttributeUsageExamples;

public class SystemTextJsonClass
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class NewtonsoftJsonClass
{
    [JsonProperty("name")]
    public string? Name { get; set; }
}
