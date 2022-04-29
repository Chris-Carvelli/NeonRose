using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace B3.Utility.Gizmos
{
    using Gizmos = UnityEngine.Gizmos;

    /// <summary>
    /// Gizmo used to draw the calculated path of a NavMeshAgent
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class PathGizmo : MonoBehaviour
    {
        [Tooltip("The color of the corner sphere gizmos.")]
        public Color SphereColor = Color.grey;
        [Tooltip("The color of the line between corner gizmos.")]
        public Color LineColor = Color.yellow;
        [Tooltip("The size of the corner sphere gizmos.")]
        public float SphereSize = 0.05f;
        /// <summary>
        /// The NavMeshAgent to read the path off of.
        /// </summary>
        private NavMeshAgent agent;

        public void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        public void OnDrawGizmos()
        {
            if (agent != null)
            {
                Vector3[] corners = agent.path.corners;
                for (int index = 0; index < corners.Length; index++)
                {
                    Gizmos.color = SphereColor;
                    Gizmos.DrawSphere(corners[index], SphereSize);
                    if(index < corners.Length - 1)
                    {
                        Gizmos.color = LineColor;
                        Gizmos.DrawLine(corners[index], corners[index + 1]);
                    }
                }
            }
        }
    }
}
