using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace B3.ActionEnums
{
    /// <summary>
    /// An enum version of the Vector3 directions.
    /// </summary>
    public enum RotationAxis
    {
        /// <summary>
        /// This will return Vector3.up
        /// </summary>
        Up,
        /// <summary>
        /// This will return the local up direction.
        /// </summary>
        UpLocal,
        /// <summary>
        /// This will return Vector3.down
        /// </summary>
        Down,
        /// <summary>
        /// This will return the local down direction.
        /// </summary>
        DownLocal,
        /// <summary>
        /// This will return Vector3.left
        /// </summary>
        Left,
        /// <summary>
        /// This will return the local left direction.
        /// </summary>
        LeftLocal,
        /// <summary>
        /// This will return Vector3.right
        /// </summary>
        Right,
        /// <summary>
        /// This will return the local right direction.
        /// </summary>
        RightLocal,
        /// <summary>
        /// This will return Vector3.back
        /// </summary>
        Back,
        /// <summary>
        /// This will return the local back direction.
        /// </summary>
        BackLocal,
        /// <summary>
        /// This will return Vector3.forward
        /// </summary>
        Forward,
        /// <summary>
        /// This will return the local forward direction.
        /// </summary>
        ForwardLocal
    }
}
