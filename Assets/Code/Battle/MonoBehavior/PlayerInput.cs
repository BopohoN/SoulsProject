using System;
using Code.InputSystemActions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Serialization;

namespace Code.Battle.MonoBehavior
{
    public class PlayerInput: MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;
        
        public bool rbInput;
        public bool rtInput;

        public event Action<InputAction.CallbackContext> OnBPressed;
        public event Action<InputAction.CallbackContext> OnBHold;
        public event Action<InputAction.CallbackContext> OnBRelease;
        
        private PlayerInputActions m_InputActions;
        private Vector2 m_MovementInput;
        private Vector2 m_CameraInput;
        private PlayerCore m_PlayerCore;

        private void Awake()
        {
            m_PlayerCore = GetComponent<PlayerCore>();
        }

        public void Start()
        {
            if (m_InputActions == null)
            {
                m_InputActions = new PlayerInputActions();
                m_InputActions.PlayerMovement.Movement.performed += ctx => m_MovementInput = ctx.ReadValue<Vector2>();
                m_InputActions.PlayerMovement.Movement.canceled += _ => m_MovementInput = Vector2.zero;

                m_InputActions.PlayerMovement.Camera.performed += ctx => m_CameraInput = ctx.ReadValue<Vector2>();
                m_InputActions.PlayerMovement.Camera.canceled += _ => m_CameraInput = Vector2.zero;

                m_InputActions.PlayerActions.RB.performed += i => rbInput = true;
                m_InputActions.PlayerActions.RB.canceled += i => rbInput = false;
                m_InputActions.PlayerActions.RT.performed += i => rtInput = true;
                m_InputActions.PlayerActions.RT.canceled += i => rtInput = false;

                m_InputActions.PlayerActions.RollAndSprint.performed += ctx =>
                {
                    if (ctx.interaction is HoldInteraction)
                        OnBHold?.Invoke(ctx);
                    else if (ctx.interaction is TapInteraction)
                        OnBPressed?.Invoke(ctx);
                };

                m_InputActions.PlayerActions.RollAndSprint.canceled += ctx =>
                {
                    OnBRelease?.Invoke(ctx);
                };
                
                m_InputActions.PlayerMovement.Enable();
                m_InputActions.PlayerActions.Enable();
            }
        }

        private void OnEnable()
        {
            m_InputActions?.PlayerMovement.Enable();
            m_InputActions?.PlayerActions.Enable();
        }

        public void OnDisable()
        {
            m_InputActions.PlayerMovement.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleAttackInput(delta);
        }

        private void MoveInput(float delta)
        {
            horizontal = m_MovementInput.x;
            vertical = m_MovementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = m_CameraInput.x;
            mouseY = m_CameraInput.y;
        }

        private void HandleAttackInput(float delta)
        {
            if (rbInput)
                m_PlayerCore.PlayerAttacker.HandleLightAttack();
            if (rtInput)
                m_PlayerCore.PlayerAttacker.HandleHeavyAttack();
        }
    }
}