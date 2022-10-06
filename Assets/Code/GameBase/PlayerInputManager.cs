using System;
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

        public event Action<InputAction.CallbackContext> OnBInput;
        
        private PlayerInputActions m_InputActions;
        private Vector2 m_MovementInput;
        private Vector2 m_CameraInput;

        public override void OnStart()
        {
            if (m_InputActions == null)
            {
                m_InputActions = new PlayerInputActions();
                m_InputActions.PlayerMovement.Movement.performed += ctx => m_MovementInput = ctx.ReadValue<Vector2>();
                m_InputActions.PlayerMovement.Movement.canceled += _ => m_MovementInput = Vector2.zero;

                m_InputActions.PlayerMovement.Camera.performed += ctx => m_CameraInput = ctx.ReadValue<Vector2>();
                m_InputActions.PlayerMovement.Camera.canceled += _ => m_CameraInput = Vector2.zero;

                m_InputActions.PlayerActions.Roll.started += ctx => OnBInput?.Invoke(ctx);
            }

            m_InputActions.PlayerMovement.Enable();
            m_InputActions.PlayerActions.Enable();
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