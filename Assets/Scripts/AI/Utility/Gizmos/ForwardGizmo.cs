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
    /// A gizmo that draws a line in the forward direction of the game objects its attached to.
    /// </summary>
    [RequireComponent(typeof(Transform))]
    public class ForwardGizmo : MonoBehaviour
    {
        [Tooltip("How long the line is.")]
        public float LineRenderLength = 0.5f;
        [Tooltip("What color to use to draw the line with.")]
        public Color LineColor = Color.red;

        public void OnDrawGizmos()
        {
            Gizmos.color = LineColor;
            Gizmos.DrawLine(transform.position + Vector3.up, (transform.position + Vector3.up) + transform.forward * LineRenderLength);
        }
    }
}
