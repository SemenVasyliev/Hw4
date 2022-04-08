using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw4.Exercise0;
[AttributeUsage(AttributeTargets.All)]
internal class NameValidationAttribute : Attribute
{
    public NameValidationAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}
