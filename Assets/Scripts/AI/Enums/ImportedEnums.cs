using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3.Enums
{
    /// <summary>
    /// Describe an axis in Cartesian coordinates, Useful for components that need to serialize which axis to use in some fashion.
    /// </summary>
    /// <remarks>
    /// Credit goes to: https://github.com/lordofduct/spacepuppy-unity-framework
    /// </remarks>
    public enum CartesianAxis
    {
        Zneg = -3,
        Yneg = -2,
        Xneg = -1,
        X = 0,
        Y = 1,
        Z = 2
    }
}
