namespace LectureExample.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
public class ToStringAttribute : Attribute
{
    private readonly string[] _props;

    public IEnumerable<string> Properties => _props;

    public ToStringAttribute(params string[] props)
    {
        _props = props ?? throw new ArgumentNullException(nameof(props));
    }
}
