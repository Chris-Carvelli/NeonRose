using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace B3.Utility.Gizmos
{
    using Gizmos = UnityEngine.Gizmos;

    /// <summary>
    /// A customizable version of the Forward Gizmo.
    /// </summary>
    [RequireComponent(typeof(Transform))]
    public class CustomForwardGizmo : MonoBehaviour
    {
        [Tooltip("The direction of the line.")]
        public Vector3 LineDirection = Vector3.forward;
        [Tooltip("How long the line is.")]
        public float LineRenderLength = 0.5f;
        [Tooltip("What color to use to draw the line with.")]
        public Color DebugColor = Color.red;

        public void OnDrawGizmos()
        {
            Gizmos.color = DebugColor;
            Gizmos.DrawLine(transform.position + transform.up, (transform.position + transform.up) + LineDirection * LineRenderLength);
        }
    }
}
