using System;
using UnityEngine;

namespace NeonRose.Animations
{
    public class CharacterAnimator : MonoBehaviour
    {
        private const float TAU = (float)Math.PI * 2;

        [Header("Drivers")]
        public bool apply;
        public Vector3 velocity;
        public Vector3 heading;

        [Header("Config")]
        public float feetRadius = .5f;
        public float handRadius = .4f;
        public float handMaxTilt = 80;
        public float handBaseTilt = 90;
        public float torsoMaxTilt = 27;

        [Header("References")]
        public Transform body;
        public Transform torso;
        public Transform rFoot;
        public Transform lFoot;
        public Transform rHand;
        public Transform lHand;

        [Header("Debug")]
        [SerializeField] private float _feetT;
        [SerializeField] private Vector3 _rOffset;
        [SerializeField] private Vector3 _lOffset;

        [SerializeField] private Vector3 _torsoBasePos;
        [SerializeField] private Vector3 _rFootBasePos;
        [SerializeField] private Vector3 _lFootBasePos;
        [SerializeField] private Vector3 _rHandBasePos;
        [SerializeField] private Vector3 _lHandBasePos;
        
        [SerializeField] private float _torsoRot;
        [SerializeField] private float _torsoTilt;
        
        [SerializeField] private float _handsTilt;

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
                heading = _controller._direction;
            
            UpdateFeet();
            UpdateTorso();
            UpdateHands();
        }

        private void UpdateTorso()
        {
            _torsoRot = Mathf.Atan2(heading.x, heading.z);
            _torsoTilt = torsoMaxTilt * (velocity.magnitude / _controller.speed);
            if (!apply)
                return;

            body.rotation = Quaternion.Euler(0, _torsoRot * Mathf.Rad2Deg, 0);
            torso.localRotation = Quaternion.Euler(_torsoTilt, 0, 0);
        }
        
        private void UpdateFeet()
        {
            
            _feetT += -Time.deltaTime * velocity.magnitude;
            _rOffset = new Vector3(
                0,
                Mathf.Max(Mathf.Sin(_feetT), 0),
                Mathf.Cos(_feetT)
            );
            
            _lOffset = new Vector3(
                0,
                Mathf.Max(Mathf.Sin(_feetT + Mathf.PI), 0),
                Mathf.Cos(_feetT + Mathf.PI)
            );
            
            if (!apply)
                return;
            
            rFoot.localPosition = _rFootBasePos + feetRadius * _rOffset;
            lFoot.localPosition = _lFootBasePos + feetRadius * _lOffset;
        }

        private void UpdateHands()
        {
            _handsTilt = handBaseTilt + handMaxTilt * (velocity.magnitude / _controller.speed);
            if (!apply)
                return;

            lHand.localRotation = Quaternion.Euler(_handsTilt, 0, 0);
            rHand.localRotation = Quaternion.Euler(_handsTilt, 0, 0);
        }
        
        [ContextMenu("RegisterLimbPositions")]
        private void RegisterLimbPositions()
        {
            var worldToLocalMatrix = torso.worldToLocalMatrix;
            
            _torsoBasePos = worldToLocalMatrix.MultiplyPoint(torso.position);
            _rFootBasePos = worldToLocalMatrix.MultiplyPoint(rFoot.position);
            _lFootBasePos = worldToLocalMatrix.MultiplyPoint(lFoot.position);
            _rHandBasePos = worldToLocalMatrix.MultiplyPoint(rHand.position);
            _lHandBasePos = worldToLocalMatrix.MultiplyPoint(lHand.position);
        }
        
        [ContextMenu("ResetLimbPositions")]
        private void ResetLimbPositions()
        {
            var localToWorldMatrix = torso.localToWorldMatrix;
            
            torso.position = localToWorldMatrix.MultiplyPoint(_torsoBasePos);
            rFoot.position = localToWorldMatrix.MultiplyPoint(_rFootBasePos);
            lFoot.position = localToWorldMatrix.MultiplyPoint(_lFootBasePos);
            rHand.position = localToWorldMatrix.MultiplyPoint(_rHandBasePos);
            lHand.position = localToWorldMatrix.MultiplyPoint(_lHandBasePos);
        }
    }
}