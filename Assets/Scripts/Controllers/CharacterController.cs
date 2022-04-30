using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NeonRose.Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterController : MonoBehaviour
    {
        private Rigidbody _body;
        private CharacterState _characterState;
        private Camera _camera;
        
        public float speed = 5.0f;
        public float jumpHeight = 10.0f;
        public float dashDistance = 3.0f;
        public float dashCd = 5.0f;
        public float dashDuration = .5f;
        
        private float stateTimer; // time a player can be in a state 

        public Vector3 _direction;

        public float angle;

        private void Start()
        {
            _characterState = CharacterState.GROUNDED;
            _camera = Camera.main;
            _body = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            angle = -_camera.transform.rotation.eulerAngles.y *  Mathf.Deg2Rad;
            UpdatePlayer();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (_characterState != CharacterState.DASHING)
            {
               var playerDirection = context.ReadValue<Vector2>();
               _direction.x = playerDirection.x;
               _direction.z = playerDirection.y;
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.started && _characterState is CharacterState.GROUNDED or CharacterState.AIRBORNE)
            {
                _characterState = CharacterState.DASHING;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {

            if (_body.velocity.y == 0 && context.started)
            { 
                _body.AddForce(0, jumpHeight, 0, ForceMode.Impulse);
                _characterState = CharacterState.AIRBORNE;
            }
        }
        
        private void UpdatePlayer()
        {
            switch (_characterState)
            {
                case CharacterState.GROUNDED:
                    HandleGroundedState();
                    break;
                case CharacterState.DASHING:
                    HandleDashingState();
                    break;
                case CharacterState.AIRBORNE:
                    HandleAirborneState();
                    break;
                case CharacterState.STOMPING:
                    HandleStompingState();
                    break;
                default:
                    throw new Exception("Unexpected player state");
            }
        }

        private void HandleGroundedState()
        {
            var correctedDirection = _direction;
            if (_direction.magnitude > 0.01f)
            {
                correctedDirection.x = Mathf.Cos(angle) * _direction.x - Mathf.Sin(angle) * _direction.z;
                correctedDirection.z = Mathf.Sin(angle) * _direction.x + Mathf.Cos(angle) * _direction.z;
            }
            Vector3 generalVelocity = correctedDirection * speed; 
            float yVelocity = _body.velocity.y;
            _body.velocity = new Vector3(generalVelocity.x, yVelocity, generalVelocity.z);
        }

        private void HandleDashingState()
        {
            _body.velocity = _direction.normalized * (speed * dashDistance);
            if (stateTimer < 0 || Time.time - stateTimer >= dashDuration)
            {
                if (_body.velocity.y == 0)
                {
                    _characterState = CharacterState.GROUNDED;
                }
                else
                {
                    _characterState = CharacterState.AIRBORNE;
                }
                stateTimer = -1;
            }
            else
            {
                stateTimer = Time.time;
            }
        }

        private void HandleAirborneState()
        {
            if (_body.velocity.y == 0)
            {
                _characterState = CharacterState.GROUNDED;
            }
        }

        private void HandleStompingState()
        {
            
        }
    }
} 