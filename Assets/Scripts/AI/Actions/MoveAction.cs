using B3.Interfaces;
using B3.Utility;
using B3.Utility.PropertyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace B3.Actions
{
    /// <summary>
    /// Very simple Action that moves the agent to a single location.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class MoveAction : SingletonMoveAction
    {
        [Header("General")]
        [Tooltip("Where the Agent should go.")]
        public GameObject TargetLocation;
        [Tooltip("Whether the AI should loop back and forth between its start location and target location or not.")]
        public bool Looping = false;
        [Tooltip("How long the AI should wait, before it moves towards its end destination. Ignored if Looping is false.")]
        public float WaitTime = 0.1f;
        [Header("Rotation")]
        [Tooltip("Whether the agent should rotate towards its steering target before it moves towards it or not.")]
        public bool RotateBeforeMove = false;
        [Tooltip("How many degrees of a difference between the agent and its Steering Target rotation before it can start moving. Only used if RotateBeforeMove is true.")]
        public float AngleThreshold = 1f;
        [Tooltip("How fast the agent spins when rotating. Only used if RotateBeforeMove is true.")]
        public float rotateSpeed = 300f;

        /// <summary>
        /// The path that the Agent generates.
        /// </summary>
        private NavMeshPath path;
        /// <summary>
        /// The agent itself.
        /// </summary>
        private NavMeshAgent agent;
        /// <summary>
        /// Only used to keep track of two locations, if Looping is true.
        /// </summary>
        private Vector3 StartLocation;

        [Header("Debug - Readonly")]
        [SerializeField]
        [Tooltip("Whether the agent successfully generated a path or not.")]
        [ReadOnly] private bool success;
        [SerializeField]
        [Tooltip("The angle between the agent and where it needs to face before moving. Only used if RotateBeforeMove is true.")]
        [ReadOnly] private float turnAngle;
        [SerializeField]
        [Tooltip("Whether the agent is currently waiting or not.")]
        [ReadOnly] private bool isWaiting;
        [SerializeField]
        [Tooltip("How long the agent has waited.")]
        [ReadOnly] private float elapsedWaitingTime = 0f;
        [SerializeField]
        [Tooltip("Whether the agent is currently on a detour.")]
        [ReadOnly] private bool isOnDetour = false;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            path = new NavMeshPath();
            if (StartLocation == null || TargetLocation == null)
            {
                this.LogError("StartLocation or TargetLocation is null. Disabling component.");
                enabled = false;
            }
            else
            {
                StartLocation = transform.position;
                agent.destination = TargetLocation.transform.position;
            }

        }

        // Update is called once per frame
        void Update()
        {
            success = agent.CalculatePath(TargetLocation.transform.position, path);
            if (Looping == true)
            {
                if (Vector3.Distance(transform.position, agent.destination) < 0.25f)
                {
                    if (isOnDetour == true)
                    {
                        agent.SetDestination(TargetLocation.transform.position);
                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, TargetLocation.transform.position) < 0.25f)
                        {
                            agent.SetDestination(StartLocation);
                        }
                        else
                        {
                            agent.SetDestination(TargetLocation.transform.position);
                        }
                    }
                    isWaiting = true;
                    elapsedWaitingTime = 0f;
                    agent.isStopped = true;
                }
            }

            if (isWaiting == false)
            {
                if (RotateBeforeMove == true)
                {
                    turnAngle = Vector3.SignedAngle(transform.forward, agent.steeringTarget - transform.position, transform.up);
                    if (turnAngle < AngleThreshold && success == true)
                    {
                        agent.isStopped = false;
                        agent.Move(Vector3.zero);
                    }
                    else
                    {
                        agent.isStopped = true;
                        Quaternion difference = Quaternion.Euler(0, turnAngle, 0);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, difference * transform.rotation, rotateSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    if (success == true)
                    {
                        agent.Move(Vector3.zero);
                    }
                }
            }
            else
            {
                if (elapsedWaitingTime < WaitTime)
                {
                    elapsedWaitingTime += Time.deltaTime;
                }
                else
                {
                    isWaiting = false;
                    agent.isStopped = false;
                }
            }
        }

        public void TakeDetour(GameObject detourPosition)
        {
            agent.destination = detourPosition.transform.position;
            isOnDetour = true;
        }
    }
}