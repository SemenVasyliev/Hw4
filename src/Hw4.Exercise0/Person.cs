namespace Hw4.Exercise0;
[NameValidation(" ")]
[AgeValidation("16,200")]
[WeightValidation("0,200")]
public sealed class Person
{
    [NameValidation(" ")]
    public string Name { get; set; }

    [AgeValidation("16,200")]
    public int Age { get; set; }

    [WeightValidation("0,200")]
    public int Weight { get; set; }

    public Person(string name, int age, int weight)
    {
        Name = name;
        Age = age;
        Weight = weight;
    }
}
