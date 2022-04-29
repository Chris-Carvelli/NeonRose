using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace B3.Actions
{
    /// <summary>
    /// If a move action extends this class, it means that any other move action that also extends it
    /// cannot coexist on the same game object. Extend this to prevent multiple move actions on
    /// the same agent that would otherwise produce undesirable results.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class SingletonMoveAction : MonoBehaviour { }

    /// <summary>
    /// If a rotation action extends this class, it means that any other rotation action that also extends it
    /// cannot coexist on the same game object. Extend this to prevent multiple rotation actions on
    /// the same agent that would otherwise produce undesirable results.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class SingletonRotationAction : MonoBehaviour { }
}
