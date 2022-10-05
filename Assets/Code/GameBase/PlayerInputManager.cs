using Code.InputSystemActions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.GameBase
{
    public class PlayerInputManager: BaseManager
    {
        public float Horizontal;
        public float Vertical;
        public float MoveAmount;
        public float MouseX;
        public float MouseY;

        private PlayerInputActions m_InputActions;
        private Vector2 m_MovementInput;
        private Vector2 m_CameraInput;
        
        public override void OnStart()
        {
            if (m_InputActions == null)
            {
                m_InputActions = new PlayerInputActions();
                m_InputActions.PlayerMovement.Movement.performed += PlayerMovement;
                m_InputActions.PlayerMovement.Movement.canceled += PlayerMovementCancel;
            }

            m_InputActions.PlayerMovement.Enable();
        }

        private void PlayerMovement(InputAction.CallbackContext ctx)
        {
            m_MovementInput = ctx.ReadValue<Vector2>();
        }

        private void PlayerMovementCancel(InputAction.CallbackContext ctx)
        {
            m_MovementInput = Vector2.zero;
        }

        public override void LogicUpdate()
        {
            MoveInput(Time.deltaTime);
        }

        public override void OnDispose()
        {
            m_InputActions.PlayerMovement.Disable();
        }

        private void MoveInput(float delta)
        {
            Horizontal = m_MovementInput.x;
            Vertical = m_MovementInput.y;
            MoveAmount = Mathf.Clamp01(Mathf.Abs(Horizontal) + Mathf.Abs(Vertical));
            MouseX = m_CameraInput.x;
            MouseY = m_CameraInput.y;
        }
    }
}