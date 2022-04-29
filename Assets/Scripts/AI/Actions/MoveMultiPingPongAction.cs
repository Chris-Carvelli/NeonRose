using B3.Utility;
using B3.Utility.PropertyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace B3.Actions
{
    /// <summary>
    /// An action that makes an agent go back and forth through a list of locations sequentially.
    /// </summary>
    public class MoveMultiPingPongAction : SingletonMoveAction
    {
        [Header("General")]
        [Tooltip("A list of locations for where the Agent should go.")]
        public GameObject[] Locations = new GameObject[3];
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

        [Header("Debug - Readonly")]
        [SerializeField]
        [Tooltip("Whether the agent successfully generated a path or not.")]
        [ReadOnly] private bool success = false;
        [SerializeField]
        [Tooltip("The angle between the agent and where it needs to face before moving. Only used if RotateBeforeMove is true.")]
        [ReadOnly] private float turnAngle = 0f;
        [SerializeField]
        [Tooltip("Whether the agent is currently waiting or not.")]
        [ReadOnly] private bool isWaiting;
        [SerializeField]
        [Tooltip("How long the agent has waited.")]
        [ReadOnly] private float elapsedWaitingTime = 0f;
        [SerializeField]
        [Tooltip("Indicates at what index in the Locations array the bot is going for next.")]
        [ReadOnly] private int locationIndex = 0;
        [SerializeField]
        [Tooltip("Whether the agent is currently going backwards in its list of locations. Ignored if IsPingPong is true.")]
        [ReadOnly] private bool IsGoingBackwards = false;

        void Start()
        {
            int nullCount = (new List<GameObject>(Locations).FindAll(x => x == null)).Count;
            int locationCount = Locations.Length - nullCount;
            if (locationCount <= 0 || locationCount == 1 || locationCount == 2)
            {
                string message = (Locations.Length == 0 ? "No locations added." : $"Only {locationCount} locations added. Minimum required is 3.");
                this.LogError($"{message}. Disabling component.");
                enabled = false;
            }
            else if (nullCount > 0)
            {
                this.LogError($"{nullCount} null location(s) found. Disabling component.");
                enabled = false;
            }

            agent = GetComponent<NavMeshAgent>();
            path = new NavMeshPath();
            locationIndex = 1;
            agent.destination = Locations[locationIndex].transform.position;
            GameObject startLocation = Instantiate(new GameObject());
            startLocation.transform.position = gameObject.transform.position;
            startLocation.name = name + "_StartLocation";
            Locations[0] = startLocation;
        }

        void Update()
        {
            success = agent.CalculatePath(Locations[locationIndex].transform.position, path);
            EvaluatePingPong();
            if (isWaiting == false)
            {
                EvaluateAgentRotation();
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

        private void EvaluatePingPong()
        {
            if (Vector3.Distance(transform.position, agent.destination) < 0.25f)
            {
                if (IsGoingBackwards == false)
                {
                    if (locationIndex + 1 < Locations.Length)
                    {
                        locationIndex += 1;
                    }
                    else
                    {
                        IsGoingBackwards = true;
                        locationIndex = Locations.Length - 1;
                    }
                }
                else
                {
                    if (locationIndex - 1 > -1)
                    {
                        locationIndex -= 1;
                    }
                    else
                    {
                        IsGoingBackwards = false;
                        locationIndex = 0;
                    }
                }
                agent.SetDestination(Locations[locationIndex].transform.position);
                isWaiting = true;
                elapsedWaitingTime = 0f;
                agent.isStopped = true;
            }
        }

        private void EvaluateAgentRotation()
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

        public void OnValidate()
        {
            if (Locations[0] == null)
            {
                Locations[0] = gameObject;
            }
            else if (!Locations[0].Equals(gameObject))
            {
#if UNITY_EDITOR
                if (!EditorApplication.isPlaying)
                {
                    Locations[0] = gameObject;
                    this.LogWarning($"The first location in the array has to be the agents own location.");
                }
#endif
            }

            if (Locations.Length < 3)
            {
                switch (Locations.Length)
                {
                    case 0:
                        Locations = new GameObject[3];
                        break;
                    case 1:
                        Locations = new GameObject[]
                        {
                            Locations[0],
                            null,
                            null
                        };
                        break;
                    case 2:
                        Locations = new GameObject[]
                        {
                            Locations[0],
                            Locations[1],
                            null
                        };
                        break;
                    default:
                        Locations = new GameObject[3];
                        break;

                }
                this.LogWarning($"The Locations array length cannot be shorter than 3. If you only need two locations, consider using a Move action instead.");
            }
        }
    }
}