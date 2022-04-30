using UnityEngine;
using UnityEngine.InputSystem;

namespace NeonRose.Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterController : MonoBehaviour
    {
        public float speed = 5.0f;
        private Rigidbody _body;
        
        public Vector3 _direction;

        private void Start()
        {
            _body = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            _body.velocity = CalculateMovement();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            var playerDirection = context.ReadValue<Vector2>();
            _direction.x = playerDirection.x;
            _direction.z = playerDirection.y;
            _direction.y = 0;
        }

        private Vector3 CalculateMovement()
        {
            return _direction * speed;
        }
    }
} 