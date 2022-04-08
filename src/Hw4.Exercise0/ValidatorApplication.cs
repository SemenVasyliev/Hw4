using Common;

namespace Hw4.Exercise0;

public sealed class ValidatorApplication
{
    /// <summary>
    /// Runs application.
    /// </summary>
    public ReturnCode Run(string[] args)
    {

        try
        {
            var person = new Person(args[0], int.Parse(args[1]), int.Parse(args[2]));
            var validationResult = PersonValidator.Validate(person);
            Console.WriteLine("Person {0} validation result is: {1}", person, validationResult);
            return validationResult.IsValid ? ReturnCode.Success : ReturnCode.InvalidArgs;
        }

        catch
        {
            return ReturnCode.InvalidArgs;
        }
    }
}
