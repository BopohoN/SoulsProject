using InputSystemActions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code
{
    public class TestingInputSystem : MonoBehaviour
    {
        private Rigidbody m_SphereRigidbody;
        private PlayerInputActions m_PlayerInputActions;

        private void Awake()
        {
            m_SphereRigidbody = GetComponent<Rigidbody>();
            m_PlayerInputActions = new PlayerInputActions();
            m_PlayerInputActions.Player.Enable();
            m_PlayerInputActions.Player.Roll.performed += Roll;
        }

        private void Roll(InputAction.CallbackContext context)
        {
            m_SphereRigidbody.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }

        private void FixedUpdate()
        {
            var inputVector = m_PlayerInputActions.Player.Movement.ReadValue<Vector2>();
            Debug.Log("inputVector = " + inputVector);
            var speed = 1f;
            m_SphereRigidbody.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);
        }
    }
}
