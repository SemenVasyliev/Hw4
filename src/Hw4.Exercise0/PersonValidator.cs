using System.Reflection;

namespace Hw4.Exercise0;

public static class PersonValidator
{
    public static PersonValidationResult Validate(Person? person)
    {
        if (person == null)
        {
            return new PersonValidationResult(false, "Person can't be null");
        }
        var t = person.GetType();

        var ageAttr = t.GetCustomAttribute<AgeValidationAttribute>();

        if (ageAttr == null)
        {
            return new PersonValidationResult(false, "Attributes can't be null");
        }
        var ageRange = ageAttr.Age.Split(',');
        int ageMin = int.Parse(ageRange[0]);
        int ageMax = int.Parse(ageRange[1]);
        if (person.Age <= ageMin || person.Age > ageMax)
        {
            return new PersonValidationResult(false, "Person Age is out of range");
        }

        var weightAttr = t.GetCustomAttribute<WeightValidationAttribute>();
        if (weightAttr == null)
        {
            return new PersonValidationResult(false, "Attributes can't be null");
        }
        var weightRange = weightAttr.Weight.Split(',');
        int weightMin = int.Parse(weightRange[0]);
        int weightMax = int.Parse(weightRange[1]);
        if (person.Weight <= weightMin || person.Weight > weightMax)
        {
            return new PersonValidationResult(false, "Person Weight is out of range");
        }

        var nameAttr = t.GetCustomAttribute<NameValidationAttribute>();
        if (nameAttr == null)
        {
            return new PersonValidationResult(false, "Attributes can't be null");
        }

        if (person.Name?.Replace(" ", "").Length <= nameAttr.Name.Length || person.Name is null)
        {
            return new PersonValidationResult(false, "Person Name is required");
        }

        return new PersonValidationResult(true, "");
    }
}
