using B3.Interfaces;
using B3.Planners;
using B3.Priorities;
using B3.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace B3.Priorities
{
    [RequireComponent(typeof(PriorityPlanner), typeof(NavMeshAgent), typeof(Rigidbody))]
    public class LookForPriority : AbstractPriority
    {
        [Tooltip("What Object in the scene to look for. Goes by Object Tag.")]
        public string Target;
        [Tooltip("What to do, once the target is found.")]
        public UnityEvent OnFoundTarget;
        [Tooltip("The collider used to detect what the agent is looking for. Will be used as a Trigger Volume.")]
        public Collider TriggerArea;
        public int layerMask;

        private NavMeshAgent agent;
        private Rigidbody rBody;
        private bool IsInvokingPriority = false;

        public void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            rBody = GetComponent<Rigidbody>();
            rBody.isKinematic = true;
            if (TriggerArea == null) { TriggerArea = GetComponent<Collider>(); }
            if (TriggerArea == null)
            {
                this.LogWarning("No Collider found on the agent. SphereCollider added to compensate.");
                TriggerArea = gameObject.AddComponent<SphereCollider>();
            }
            TriggerArea.isTrigger = true;
        }

        /// <summary>
        /// Called by the <see cref="PriorityPlanner"/> to invoke this priority.
        /// </summary>
        public override void InvokePriority()
        {
            IsInvokingPriority = true;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Target))
            {
                Ray ray = new Ray(transform.position, transform.position - other.gameObject.transform.position);
                float distance = Vector3.Distance(transform.position, other.gameObject.transform.position);
                RaycastHit[] castResult = Physics.RaycastAll(ray, distance, layerMask, QueryTriggerInteraction.Collide);
                if (castResult.Length == 1 && castResult[0].collider.gameObject.CompareTag(Target))
                {
                    OnFoundTarget?.Invoke();
                    IsInvokingPriority = false;
                    InvokeOnDone(this, null);
                }
            }
        }
    }
}