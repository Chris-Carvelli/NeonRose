using System;
using UnityEditor;
using UnityEngine;

namespace NeonRose.Animations
{
    [ExecuteAlways]
    public class CharacterAnimator : MonoBehaviour
    {
        private const float TAU = (float)Math.PI * 2;

        [Header("Drivers")]
        public bool update;
        public Vector3 speed;

        [Header("Config")]
        public float feetRadius = .5f;

        [Header("References")]
        public Transform foot;

        [Header("Debug")]
        [SerializeField] private float _t;
        [SerializeField] private float _angle;
        [SerializeField] private Vector3 _offset;

        [SerializeField] private Vector3 _footBasePos;

        private void Start()
        {
            
        }

        private void Update()
        {
            _angle = Time.time * speed.x;
            _offset = new Vector3(
                0,
                Mathf.Max(Mathf.Sin(_angle), 0),
                Mathf.Cos(_angle)
            );
            if (!update)
                return;
            
            foot.localPosition = _footBasePos + feetRadius * _offset;
        }

        [ContextMenu("RegisterLimbPositions")]
        private void RegisterLimbPositions()
        {
            _footBasePos = transform.worldToLocalMatrix.MultiplyPoint(foot.position);
        }
        
        [ContextMenu("ResetLimbPositions")]
        private void ResetLimbPositions()
        {
            foot.position = transform.localToWorldMatrix.MultiplyPoint(_footBasePos);
        }
    }
}