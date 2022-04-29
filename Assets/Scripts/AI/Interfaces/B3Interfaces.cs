using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3.Interfaces
{
    public interface IPriority
    {
        event EventHandler OnDone;
        void InvokePriority();
    }

    /// <summary>
    /// Credit to https://github.com/lordofduct/spacepuppy-unity-framework
    /// </summary>
    public interface IRandom
    {
        float Next();
        double NextDouble();
        int Next(int size);
        int Next(int low, int high);
    }
}
