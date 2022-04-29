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
    /// A Gizmo used to show a NavMeshAgent's current Steering Target.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class SteeringTargetGizmo : MonoBehaviour
    {
        [Tooltip("The color of the Steering Target sphere gizmo.")]
        public Color SphereColor = Color.green;
        [Tooltip("The size of the Steering Target sphere gizmo.")]
        public float SphereSize = 0.125f;
        [Tooltip("The path and name of the Icon to show in the Assets/Gizmo folder. Leave blank if none.")]
        public string GizmoIconName = "CrosshairGizmo.png";
        [Tooltip("The color tint of the Icon.")]
        public Color GizmoIconColor = Color.red;
        [Tooltip("The multiplication factor for how far above the gizmo the icon should appear.")]
        public float GizmoIconDistance = 0.25f;
        /// <summary>
        /// The agent to read off of.
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
                if (!GizmoIconName.Equals(string.Empty))
                {
                    Gizmos.DrawIcon(agent.steeringTarget + (Vector3.up * GizmoIconDistance), GizmoIconName, true, GizmoIconColor);
                }
                Gizmos.color = SphereColor;
                Gizmos.DrawSphere(agent.steeringTarget, SphereSize);
            }
        }
    }
}
