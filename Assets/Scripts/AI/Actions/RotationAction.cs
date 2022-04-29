using B3.ActionEnums;
using B3.Utility;
using B3.Utility.PropertyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace B3.Actions
{
    /// <summary>
    /// This action will have the agent rotating in place around a provided axis.
    /// </summary>
    public class RotationAction : SingletonRotationAction
    {
        [Header("General")]
        [Tooltip("The axis to rotate around.")]
        public RotationAxis Axis = RotationAxis.Up;
        [Tooltip("The agent will rotate in the positive direction by default. Set this to true to rotate in the negative direction.")]
        public bool IsReverse = false;
        [Tooltip("How how much the agent rotates per frame.")]
        public float RotationSpeed = 100f;
        [Tooltip("How long, if at all, the agent should wait with rotating after a full rotation.")]
        public float WaitTime = 0.0f;

        /// <summary>
        /// Keeps track of the previous axis, in case the current axis changes.
        /// </summary>
        private RotationAxis oldAxis;
        /// <summary>
        /// Keeps track of the previous reverse state, in case the currect state changes.
        /// </summary>
        private bool oldIsReverse;
        /// <summary>
        /// POtential attached agent to this game object.
        /// </summary>
        private NavMeshAgent agent;

        [Header("Debug - Readonly")]
        [SerializeField]
        [Tooltip("For how many seconds the agent has waited since it stopped to wait.")]
        [ReadOnly] private float elapsedTime = 0.0f;
        [SerializeField]
        [Tooltip("Whether the agent is currently waiting or not.")]
        [ReadOnly] private bool isWaiting = false;
        [SerializeField]
        [Tooltip("The resulting axis vector derived from the Axis Enum field.")]
        [ReadOnly] private Vector3 axisVector;
        [SerializeField]
        [Tooltip("The agents current accumulated rotation. Resets on Axis change.")]
        [ReadOnly] private float rotationValue;

        void Start()
        {
            oldAxis = Axis;
            oldIsReverse = IsReverse;
            rotationValue = 0f;
            axisVector = EnumUtility.GetRotationAxisVector(Axis, transform);
            agent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            if (Axis != oldAxis)
            {
                if(agent != null)
                {
                    // if there is a NavMeshAgent it has to be disabled if the rotation is anything
                    // but the up rotation. This is because the NavMeshAgent always forces the Up 
                    // direction for its rotation to be consistent.
                    if (Axis == RotationAxis.Up || Axis == RotationAxis.UpLocal)
                    {
                        agent.enabled = true;
                    }
                    else
                    {
                        agent.enabled = false;
                    }
                }
                oldAxis = Axis;
                axisVector = EnumUtility.GetRotationAxisVector(Axis, transform);
                rotationValue = 0f;
            }
            else if (IsReverse != oldIsReverse)
            {
                oldIsReverse = IsReverse;
                rotationValue = 0f;
            }

            if (isWaiting == false)
            {
                if (IsReverse == false)
                {
                    rotationValue += RotationSpeed * Time.deltaTime;
                    transform.Rotate(axisVector, RotationSpeed * Time.deltaTime);
                }
                else
                {
                    rotationValue += RotationSpeed * Time.deltaTime;
                    transform.Rotate(-axisVector, RotationSpeed * Time.deltaTime);
                }
            }
            else
            {
                if (WaitTime > 0f)
                {
                    if (elapsedTime >= WaitTime)
                    {
                        elapsedTime = 0f;
                        isWaiting = false;
                    }
                    else
                    {
                        elapsedTime += Time.deltaTime;
                    }
                }
            }

            if (Mathf.Approximately(rotationValue, 360f) || rotationValue > 360f)
            {
                rotationValue = 0f;
                isWaiting = true;
            }
        }
    }
}