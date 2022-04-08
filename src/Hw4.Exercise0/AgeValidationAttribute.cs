using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw4.Exercise0;
[AttributeUsage(AttributeTargets.All)]
internal class AgeValidationAttribute : Attribute
{
    public AgeValidationAttribute(string age)
    {
        Age = age;
    }

    public string Age { get; set; }
}
