using System;
using Code.GameBase;
using Code.Utility;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInputManager = Code.GameBase.PlayerInputManager;

namespace Code
{
    public class PlayerController : MonoBehaviour
    {
        private Transform m_Camera;
        private Vector3 m_MoveDir;
        
        [Header("Stats")]
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float sprintingSpeed = 7f;
        [SerializeField] private float rotationSpeed = 10f;
        private Rigidbody m_RigidBody;
        private AnimatorController m_AnimatorController;
        private PlayerInputManager m_InputManager;

        private bool m_IsSprinting;

        private void Start()
        {
            m_RigidBody = GetComponent<Rigidbody>();
            m_AnimatorController = transform.GetChild(0).GetComponent<AnimatorController>();
            m_AnimatorController.Initialize(OnAnimatorMovement);
            m_InputManager = GameManager.PlayerInputManager;
            m_Camera = Camera.main.transform;

            m_InputManager.OnBPressed += HandleRollingAndSprinting;
            m_InputManager.OnBHold += HandleSprint;
            m_InputManager.OnBRelease += ResetRollAndSprint;
        }

        private void OnAnimatorMovement(Animator animator)
        {
            m_RigidBody.drag = 0f;
            var deltaPosition = animator.deltaPosition;
            m_RigidBody.velocity = new Vector3(deltaPosition.x, 0, deltaPosition.z) / Time.deltaTime;
        }

        private Vector3 m_NormalVector;
        private Vector3 m_TargetPosition;

        private void Update()
        {
            HandleMovement(Time.deltaTime);
            m_AnimatorController.SetCanRotate(!m_AnimatorController.IsInteractingFlag);
        }

        private void HandlerRotation(float delta)
        {
            var moveOverride = m_InputManager.Vertical;

            var targetDir = m_Camera.forward * m_InputManager.Vertical;
            targetDir += m_Camera.right * m_InputManager.Horizontal;
            
            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
                targetDir = transform.forward;

            var rs = rotationSpeed;
            var tr = Quaternion.LookRotation(targetDir);
            var targetRotation = Quaternion.Slerp(transform.rotation, tr, rs * delta);

            transform.rotation = targetRotation;
        }

        private void HandleMovement(float delta)
        {
            m_MoveDir = m_Camera.forward * m_InputManager.Vertical;
            m_MoveDir += m_Camera.right * m_InputManager.Horizontal;
            m_MoveDir.Normalize();

            var speed = movementSpeed;
            var moveAmount = MovementUtility.ClampMovement(m_InputManager.MoveAmount);
            m_MoveDir *= m_IsSprinting ? sprintingSpeed : speed * moveAmount;

            var projectedVelocity = Vector3.ProjectOnPlane(new Vector3(m_MoveDir.x, 0, m_MoveDir.z), m_NormalVector);
            m_RigidBody.velocity = projectedVelocity;

            m_AnimatorController.UpdateAnimatorValue(m_InputManager.MoveAmount, 0, m_IsSprinting);
            if (m_AnimatorController.CanRotate)
                HandlerRotation(delta);
        }
        
        private void ResetRollAndSprint(InputAction.CallbackContext ctx)
        {
            m_IsSprinting = false;
        }

        private void HandleSprint(InputAction.CallbackContext ctx)
        {
            m_IsSprinting = true;
        }
        
        private void HandleRollingAndSprinting(InputAction.CallbackContext ctx)
        {
            if (m_AnimatorController.IsInteractingFlag) return;

            m_MoveDir = m_Camera.forward * m_InputManager.Vertical;
            m_MoveDir += m_Camera.right * m_InputManager.Horizontal;

            if (m_InputManager.MoveAmount > 0)
            {
                m_AnimatorController.PlayTargetAnimation("Rolling", true);
                transform.rotation = Quaternion.LookRotation(new Vector3(m_MoveDir.x, 0f, m_MoveDir.z));
            }
            else
            {
                m_AnimatorController.PlayTargetAnimation("BackStep", true);
            }
        }

        private void OnDestroy()
        {
            m_InputManager.OnBPressed -= HandleRollingAndSprinting;
        }
    }
}