using System;
using UnityEngine;

namespace NeonRose.Animations
{
    [ExecuteAlways]
    public class CharacterAnimator : MonoBehaviour
    {
        private const float TAU = (float)Math.PI * 2;

        [Header("Drivers")]
        public bool apply;
        public Vector3 velocity;
        public Vector3 heading;

        [Header("Config")]
        public float feetRadius = .5f;

        [Header("References")]
        public Transform torso;
        public Transform rFoot;
        public Transform lFoot;

        [Header("Debug")]
        [SerializeField] private float _angle;
        [SerializeField] private Vector3 _rOffset;
        [SerializeField] private Vector3 _lOffset;

        [SerializeField] private Vector3 _torsoBasePos;
        [SerializeField] private Vector3 _rFootBasePos;
        [SerializeField] private Vector3 _lFootBasePos;
        
        [SerializeField] private float _atan2;
        [SerializeField] private Quaternion _torsoRot;

        private Rigidbody _body;
        private NeonRose.Controllers.CharacterController _controller;

        private void Start()
        {
            _body       = GetComponent<Rigidbody>();
            _controller = GetComponent<NeonRose.Controllers.CharacterController>();
        }
        
        private void Update()
        {
            velocity = _body.velocity;
            if (_controller._direction.magnitude > .001f)
                heading.x = _controller._direction.x;
            
            UpdateFeet();
            UpdateTorso();
        }

        private void UpdateTorso()
        {
            _atan2 = Mathf.Atan2(heading.x, heading.z);
            
            if (!apply)
                return;
            
            torso.rotation = _torsoRot = Quaternion.Euler(0, _atan2 * Mathf.Rad2Deg, 0);
        }
        
        private void UpdateFeet()
        {
            
            _angle += -Time.deltaTime * velocity.magnitude;
            _rOffset = new Vector3(
                0,
                Mathf.Max(Mathf.Sin(_angle), 0),
                Mathf.Cos(_angle)
            );
            
            _lOffset = new Vector3(
                0,
                Mathf.Max(Mathf.Sin(_angle + Mathf.PI), 0),
                Mathf.Cos(_angle + Mathf.PI)
            );
            
            if (!apply)
                return;
            
            rFoot.localPosition = _rFootBasePos + feetRadius * _rOffset;
            lFoot.localPosition = _lFootBasePos + feetRadius * _lOffset;
        }
        
        [ContextMenu("RegisterLimbPositions")]
        private void RegisterLimbPositions()
        {
            var worldToLocalMatrix = torso.worldToLocalMatrix;
            
            _torsoBasePos = worldToLocalMatrix.MultiplyPoint(torso.position);
            _rFootBasePos = worldToLocalMatrix.MultiplyPoint(rFoot.position);
            _lFootBasePos = worldToLocalMatrix.MultiplyPoint(lFoot.position);
        }
        
        [ContextMenu("ResetLimbPositions")]
        private void ResetLimbPositions()
        {
            var localToWorldMatrix = torso.localToWorldMatrix;
            
            torso.position = localToWorldMatrix.MultiplyPoint(_torsoBasePos);
            rFoot.position = localToWorldMatrix.MultiplyPoint(_rFootBasePos);
            lFoot.position = localToWorldMatrix.MultiplyPoint(_lFootBasePos);
        }
    }
}