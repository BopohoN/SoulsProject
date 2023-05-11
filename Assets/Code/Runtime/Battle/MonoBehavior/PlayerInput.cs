using System;
using System.Collections;
using Code.Configuration;
using Code.InputSystemActions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Code.Runtime.Battle.MonoBehavior
{
    public class PlayerInput : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool rbInput;
        public bool rbInputBuffer;
        public bool rtInput;
        public bool rtInputBuffer;

        private Vector2 m_CameraInput;

        private PlayerInputActions m_InputActions;
        private Vector2 m_MovementInput;
        private PlayerCore m_PlayerCore;

        private void Awake()
        {
            m_PlayerCore = GetComponent<PlayerCore>();
        }
        
        private enum EButton
        {
            Rb,
            Rt,
            B
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

                m_InputActions.PlayerActions.RB.performed += i =>
                {
                    rbInput = true;
                    StartCoroutine(PressButton(EButton.Rb));
                };
                m_InputActions.PlayerActions.RB.canceled += i =>
                {
                    rbInput = false;
                };
                m_InputActions.PlayerActions.RT.performed += i =>
                {
                    rtInput = true;
                    StartCoroutine(PressButton(EButton.Rt));
                };
                m_InputActions.PlayerActions.RT.canceled += i =>
                {
                    rtInput = false;
                };

                m_InputActions.PlayerActions.RollAndSprint.performed += ctx =>
                {
                    if (ctx.interaction is HoldInteraction)
                        OnBHold?.Invoke(ctx);
                    else if (ctx.interaction is TapInteraction)
                        OnBPressed?.Invoke(ctx);
                };

                m_InputActions.PlayerActions.RollAndSprint.canceled += ctx => { OnBRelease?.Invoke(ctx); };

                m_InputActions.PlayerQuickInventory.DPadLeft.performed += ctx =>
                {
                    if (ctx.interaction is HoldInteraction)
                        OnDLeftHold?.Invoke(ctx);
                    else if (ctx.interaction is TapInteraction)
                        OnDLeftPressed?.Invoke(ctx);
                };
                m_InputActions.PlayerQuickInventory.DPadRight.performed += ctx =>
                {
                    if (ctx.interaction is HoldInteraction)
                        OnDRightHold?.Invoke(ctx);
                    else if (ctx.interaction is TapInteraction)
                        OnDRightPressed?.Invoke(ctx);
                };
                
                m_InputActions.PlayerMovement.Enable();
                m_InputActions.PlayerActions.Enable();
                m_InputActions.PlayerQuickInventory.Enable();
            }
        }

        private IEnumerator PressButton(EButton button)
        {
            switch (button)
            {
                case EButton.Rb:
                    rbInputBuffer = true;
                    rtInputBuffer = false;
                    break;
                case EButton.Rt:
                    rbInputBuffer = false;
                    rtInputBuffer = true;
                    break;
                case EButton.B:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(button), button, null);
            }

            yield return new WaitForSeconds(ConstConfig.D[10012].Value / 1000f);
            
            switch (button)
            {
                case EButton.Rb:
                    rbInputBuffer = false;
                    break;
                case EButton.Rt:
                    rtInputBuffer = false;
                    break;
                case EButton.B:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(button), button, null);
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

        public event Action<InputAction.CallbackContext> OnBPressed;
        public event Action<InputAction.CallbackContext> OnBHold;
        public event Action<InputAction.CallbackContext> OnBRelease;
        public event Action<InputAction.CallbackContext> OnDLeftPressed;
        public event Action<InputAction.CallbackContext> OnDLeftHold;
        public event Action<InputAction.CallbackContext> OnDRightPressed;
        public event Action<InputAction.CallbackContext> OnDRightHold;

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
            if (m_PlayerCore.canDoCombo)
            {
                if (rbInputBuffer)
                {
                    m_PlayerCore.PlayerAttacker.HandleWeaponCombo(m_PlayerCore.PlayerInventory.RightWeapon,
                        true);
                }
                
                if (rtInputBuffer)
                {
                    m_PlayerCore.PlayerAttacker.HandleWeaponCombo(m_PlayerCore.PlayerInventory.RightWeapon,
                        false);
                }
            }
            else
            {
                if (m_PlayerCore.isInteracting)
                    return;
                if (rbInputBuffer)
                    m_PlayerCore.PlayerAttacker.HandleLightAttack(m_PlayerCore.PlayerInventory.RightWeapon);
                if (rtInputBuffer)
                    m_PlayerCore.PlayerAttacker.HandleHeavyAttack(m_PlayerCore.PlayerInventory.RightWeapon);
            }
        }
    }
}