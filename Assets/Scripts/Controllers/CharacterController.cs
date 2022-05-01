using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NeonRose.Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterController : MonoBehaviour
    {
        private Rigidbody _body;
        public CharacterState _characterState;
        private Camera _camera;
        
        public float speed = 10.0f;
        public float jumpHeight = 10.0f;
        public float dashDistance = 3.0f;
        public float dashCd = 5.0f;
        public float dashDuration = .5f;
        public float raycastDistance = 1f;
        public float groundRaycastDistance = 1.1f;
        
        private float stateTimer; // time a player can be in a state 
        public Vector3 _direction;
        public float angle;
        
        public bool _canClimb;
        public bool _isGrounded;

        private void Start()
        {
            _characterState = CharacterState.GROUNDED;
            _camera = Camera.main;
            _body = GetComponent<Rigidbody>();
            _canClimb = false;
            _isGrounded = true;
        }

        private void FixedUpdate()
        {
            HandleRaycast();
            angle = -_camera.transform.rotation.eulerAngles.y *  Mathf.Deg2Rad;
            UpdatePlayer();
        }

        private void HandleRaycast()
        {
            var pos = transform.position;
            
            var fwd = transform.TransformDirection(Vector3.forward);
            _canClimb = Physics.Raycast(pos, fwd, raycastDistance);

            var ground = transform.TransformDirection(Vector3.down);
            _isGrounded = Physics.Raycast(pos, ground, groundRaycastDistance);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (_characterState != CharacterState.DASHING)
            {
                MoveCharacter(context.ReadValue<Vector2>());
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
            if (context.started && _isGrounded)
            {
                _body.AddForce(0, jumpHeight, 0, ForceMode.Impulse);
                _characterState = CharacterState.AIRBORNE;
            }
        }

        public void OnClimb(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _body.useGravity = true;
                _characterState = CharacterUtils.GetDefaultState(_isGrounded);
            } else if (context.started && _canClimb)
            {
                _body.useGravity = false;
                _characterState = CharacterState.CLIMBING;
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
                case CharacterState.CLIMBING:
                    HandleClimbingState();
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
                _characterState = CharacterUtils.GetDefaultState(_isGrounded);
                stateTimer = -1;
            }
            else
            {
                stateTimer = Time.time;
            }
        }

        private void HandleAirborneState()
        {
            if (_isGrounded && _characterState != CharacterState.DASHING)
            {
                _characterState = CharacterState.GROUNDED;
            }
        }

        private void HandleClimbingState()
        {
            if (!_canClimb)
            {
                _characterState = CharacterUtils.GetDefaultState(_isGrounded);
            }
        }

        private void MoveCharacter(Vector2 playerDirection)
        {
            _direction.x = playerDirection.x;
            if (_characterState == CharacterState.GROUNDED)
            {
                _direction.z = playerDirection.y;

            } else if (_characterState == CharacterState.CLIMBING)
            {
                _direction.y = playerDirection.y;
            }
        }
    }
} 