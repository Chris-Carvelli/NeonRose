using B3.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace B3.Priorities
{
    public abstract class AbstractPriority : MonoBehaviour, IPriority
    {
        public event EventHandler OnDone;

        [Range(0f, 1f)]
        public float Weight;

        public abstract void InvokePriority();
        public virtual void InvokeOnDone(AbstractPriority caller, EventArgs e)
        {
            OnDone?.Invoke(caller, e);
        }
    }
}
