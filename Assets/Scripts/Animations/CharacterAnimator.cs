using System.Collections.Generic;
using NeonRose.Controllers;
using UnityEngine;

namespace NeonRose.Animations
{
    public class CharacterAnimator : MonoBehaviour
    {
        private Dictionary<CharacterState, IAnims> _anims = new ()
        {
            { CharacterState.GROUNDED, new WalkAnims() },
            { CharacterState.AIRBORNE, new AirborneAnims() }
        };
        
        [Header("References")]
        public Transform body;
        public Transform torso;
        public Transform rFoot;
        public Transform lFoot;
        public Transform rHand;
        public Transform lHand;

        [Header("Debug")]
        public CharacterState state;
        public Vector3 _torsoBasePos;
        public Vector3 _rFootBasePos;
        public Vector3 _lFootBasePos;
        public Vector3 _rHandBasePos;
        public Vector3 _lHandBasePos;

        public Rigidbody _body;
        public Controllers.CharacterController _controller;

        private void Start()
        {
            _body       = GetComponent<Rigidbody>();
            _controller = GetComponent<Controllers.CharacterController>();
        }

        private void Update()
        {
            if (_anims.ContainsKey(_controller._characterState))
                _anims[_controller._characterState].Update(this);
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