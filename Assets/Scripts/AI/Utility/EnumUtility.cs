using B3.ActionEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace B3.Utility
{
    public static class EnumUtility
    {
        /// <summary>
        /// Utility method used to get the corresponding Vector3 direction from the given <see cref="RotationAxis"/>.
        /// </summary>
        /// <param name="axis">The axis to get.</param>
        /// <param name="transform">Needed when using any of the Local versions of the enum.</param>
        /// <returns>The requested axis as a Vector3. If Local versions are used, it will return the transforms Axis instead. If transform is null, defaults to Vector3 axis.</returns>
        public static Vector3 GetRotationAxisVector(RotationAxis axis, Transform transform = null)
        {
            switch (axis)
            {
                case RotationAxis.Up:
                    return Vector3.up;
                case RotationAxis.UpLocal:
                    return (transform == null ? Vector3.up : transform.up);
                case RotationAxis.Down:
                    return Vector3.down;
                case RotationAxis.DownLocal:
                    return (transform == null ? Vector3.down : -transform.up);
                case RotationAxis.Left:
                    return Vector3.left;
                case RotationAxis.LeftLocal:
                    return (transform == null ? Vector3.left : -transform.right);
                case RotationAxis.Right:
                    return Vector3.right;
                case RotationAxis.RightLocal:
                    return (transform == null ? Vector3.right : transform.right);
                case RotationAxis.Back:
                    return Vector3.back;
                case RotationAxis.BackLocal:
                    return (transform == null ? Vector3.back : -transform.forward);
                case RotationAxis.Forward:
                    return Vector3.forward;
                case RotationAxis.ForwardLocal:
                    return (transform == null ? Vector3.forward : transform.forward);
                default:
                    Logging.LogWarningGeneral(typeof(EnumUtility), "A RotationAxis enum value was given that triggered the default case. Returning Vector3.forward.", "GetRotationAxisVector");
                    return Vector3.forward;
            }
        }
    }
}
