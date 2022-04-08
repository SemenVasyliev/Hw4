using System.Diagnostics;
using LectureExample.Attributes;

namespace LectureExample;

[ToString(nameof(Name), nameof(Age), nameof(Father), nameof(Mother))]
public class Person
{
    public string? Name { get; set; }
    public int Age { get; set; }
    public Person? Father { get; set; }
    public Person? Mother { get; set; }
}

[DebuggerDisplay("Name: {Name}, Age: {Age}")]
public class PersonDebug : Person
{
}

public class PersonToString : Person
{
    public override string? ToString()
    {
        return $"Name: {Name}, Age: {Age}";
    }
}
