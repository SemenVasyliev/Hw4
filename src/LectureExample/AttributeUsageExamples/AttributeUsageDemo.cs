namespace LectureExample.AttributeUsageExamples;

[AttributeUsage(AttributeTargets.Property|AttributeTargets.Parameter|AttributeTargets.Class)]
public class WarningAttribute: Attribute
{
    public WarningAttribute()
    {
    }

    public WarningAttribute(string message)
    {
        Message = message;
    }

    public string Message { get; set; }
}

[Warning("Age can't be less than 0 years.")]
public class Person
{
    public string? Name { get; set; }

    [Warning(Message = "Age can't be less than 0 years.")]
    public int Age { get; set; }

    public void DoSomething([Warning] string parameter)
    {
    }
}
