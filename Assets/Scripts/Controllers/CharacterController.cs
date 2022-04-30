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
        public float dashDistance = 3.0f;
        public float dashCd = 5.0f;
        public float dashDuration = .5f;

        private float stateTimer; // time a player can be in a state 

        public Vector3 _direction;

        private void Start()
        {
            _characterState = CharacterState.GROUNDED;
            _camera = Camera.main;
            _body = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            UpdatePlayer();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (_characterState != CharacterState.DASHING)
            {
                       var playerDirection = context.ReadValue<Vector2>();
                            _direction.x = playerDirection.x;
                            _direction.z = playerDirection.y;
                            _direction.y = 0;
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.started && _characterState == CharacterState.GROUNDED)
            {
                _characterState = CharacterState.DASHING;
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
            _body.velocity = _direction * speed;

        }

        private void HandleDashingState()
        {
            _body.velocity = _direction.normalized * (speed * dashDistance);
            if (stateTimer < 0 || Time.time - stateTimer >= dashDuration)
            {
                _characterState = CharacterState.GROUNDED;
                stateTimer = -1;
            }
            else
            {
                stateTimer = Time.time;
            }
        }

        private void HandleAirborneState()
        {
            
        }

        private void HandleStompingState()
        {
            
        }
    }
} 