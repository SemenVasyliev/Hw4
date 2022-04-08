namespace Hw4.Exercise0;

public sealed class PersonValidationResult
{
    public PersonValidationResult(bool isValid, string? errorMessage)
    {
        IsValid = isValid;
        ErrorMessage = errorMessage;
    }
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }
}
