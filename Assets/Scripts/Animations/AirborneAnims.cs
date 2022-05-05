using System;
using UnityEngine;

namespace NeonRose.Animations
{
    [Serializable]
    public class AirborneAnims : IAnims
    {
        
        [Header("Drivers")]
        public Vector3 velocity;
        public Vector3 heading;

        [Header("Config")]
        public float torsoRotSpeed = 20;
        
        private float _feetT;
        private Vector3 _rOffset;
        private Vector3 _lOffset;
        
        private float _torsoRot;
        private float _torsoTilt;
        private float _handsTilt;
        
        public void Update(CharacterAnimator ctx)
        {
            velocity = ctx._body.velocity;
            if (ctx._controller._direction.magnitude > .001f)
                heading = ctx._controller._direction;
            
            UpdateTorso(ctx);
            UpdateFeet(ctx);
            UpdateHands(ctx);
        }

        private void UpdateTorso(CharacterAnimator ctx)
        {
            _torsoRot = Mathf.Atan2(heading.x, heading.z);
            
            ctx.body.rotation = Quaternion.Euler(0, _torsoRot * Mathf.Rad2Deg, 0);
            ctx.torso.localRotation = Quaternion.Euler(0, 0, Time.deltaTime * torsoRotSpeed);
        }
        
        private void UpdateFeet(CharacterAnimator ctx)
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
            
            // ctx.rFoot.localPosition = ctx._rFootBasePos + feetRadius * _rOffset;
            // ctx.lFoot.localPosition = ctx._lFootBasePos + feetRadius * _lOffset;
        }

        private void UpdateHands(CharacterAnimator ctx)
        {
            // _handsTilt = handBaseTilt + handMaxTilt * (velocity.magnitude / ctx._controller.speed);

            ctx.lHand.localRotation = Quaternion.Euler(_handsTilt, 0, 0);
            ctx.rHand.localRotation = Quaternion.Euler(_handsTilt, 0, 0);
        }
    }
}