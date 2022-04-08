using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw4.Exercise0;
[AttributeUsage(AttributeTargets.All)]
internal class WeightValidationAttribute : Attribute
{
    public string Weight { get; set; }
    public WeightValidationAttribute(string weight)
    {
        Weight = weight;
    }
}
