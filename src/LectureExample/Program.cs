using LectureExample;
using LectureExample.Extensions;

static T GetPerson<T>() where T : Person, new()
    => new()
    {
        Age = 29,
        Name = "Max",
        Father = new T { Age = 51, Name = "John" },
        Mother = new T { Age = 50, Name = "Anne" }
    };

var p1 = GetPerson<Person>();
var p2 = GetPerson<PersonDebug>();
var p3 = GetPerson<PersonToString>();

Console.WriteLine("p1: {0}", p1);
Console.WriteLine("p2: {0}", p2);
Console.WriteLine("p3: {0}", p3);

// Stack overflow
// max.Father.Father = max;

// print info using our custom attribute
Console.WriteLine("Person is: {0}", p1.ToStringExt());

// because we've implemented .ToString() ...
// Console.WriteLine("Person is: {0}", max);
