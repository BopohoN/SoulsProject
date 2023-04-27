using Code.Runtime.Utility;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Runtime.Battle.MonoBehavior
{
    public class PlayerController : MonoBehaviour
    {
        private const float GroundDetectPointOffset = 0.3f;
        private const float GroundDetectMinimumDistance = 0.5f;

        [Header("Stats")]
        [SerializeField] private float walkSpeed = 1.5f;
        [SerializeField] private float fullMoveSpeed = 4.5f;
        [SerializeField] private float sprintingSpeed = 6f;
        [SerializeField] private float rotationSpeed = 10f;
        private Transform m_Camera;
        private float m_FallingTimer;
        private LayerMask m_IgnoreLayerMask;
        private Vector3 m_MoveDir;

        private Vector3 m_NormalVector;
        private PlayerCore m_PlayerCore;
        private Rigidbody m_RigidBody;
        private Vector3 m_TargetPosition;

        private void Start()
        {
            m_RigidBody = GetComponent<Rigidbody>();
            m_PlayerCore = GetComponent<PlayerCore>();
            m_PlayerCore.AnimatorController.Initialize(OnAnimatorMovement);
            m_FallingTimer = 0f;
            m_Camera = Camera.main.transform;
            m_IgnoreLayerMask = ~((1 << 9) | (1 << 10));

            m_PlayerCore.PlayerInput.OnBPressed += HandleRollingAndSprinting;
            m_PlayerCore.PlayerInput.OnBHold += HandleSprint;
            m_PlayerCore.PlayerInput.OnBRelease += ResetRollAndSprint;
        }

        private void OnDestroy()
        {
            m_PlayerCore.PlayerInput.OnBPressed -= HandleRollingAndSprinting;
        }

        private void OnAnimatorMovement(Animator animator)
        {
            m_RigidBody.drag = 0f;
            var deltaPosition = animator.deltaPosition;
            m_RigidBody.velocity = new Vector3(deltaPosition.x, 0, deltaPosition.z) / Time.deltaTime;
        }

        private void HandlerRotation(float delta)
        {
            var targetDir = m_Camera.forward * m_PlayerCore.PlayerInput.vertical;
            targetDir += m_Camera.right * m_PlayerCore.PlayerInput.horizontal;

            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
                targetDir = transform.forward;

            var rs = rotationSpeed;
            var tr = Quaternion.LookRotation(targetDir);
            var targetRotation = Quaternion.Slerp(transform.rotation, tr, rs * delta);

            transform.rotation = targetRotation;
        }

        public void HandleMovement(float delta)
        {
            m_MoveDir = m_Camera.forward * m_PlayerCore.PlayerInput.vertical;
            m_MoveDir += m_Camera.right * m_PlayerCore.PlayerInput.horizontal;
            m_MoveDir.Normalize();

            var moveAmount = MovementUtility.ClampMovement(m_PlayerCore.PlayerInput.moveAmount);
            if (moveAmount < 0.4f || !m_PlayerCore.isSprinting)
                ResetRollAndSprint(default);
            
            var speed = moveAmount >= 0.8f ? fullMoveSpeed : walkSpeed;
            m_MoveDir *= m_PlayerCore.isSprinting ? sprintingSpeed : speed;

            var projectedVelocity = Vector3.ProjectOnPlane(new Vector3(m_MoveDir.x, 0, m_MoveDir.z), m_NormalVector);
            m_RigidBody.velocity = projectedVelocity;

            m_PlayerCore.AnimatorController.UpdateAnimatorValue(m_PlayerCore.PlayerInput.moveAmount, 0,
                m_PlayerCore.isSprinting);
            if (m_PlayerCore.AnimatorController.CanRotate)
                HandlerRotation(delta);
        }

        private void ResetRollAndSprint(InputAction.CallbackContext ctx)
        {
            m_PlayerCore.isSprinting = false;
        }

        private void HandleSprint(InputAction.CallbackContext ctx)
        {
            if (m_PlayerCore.PlayerInput.moveAmount > 0.55f)
                m_PlayerCore.isSprinting = true;
        }

        private void HandleRollingAndSprinting(InputAction.CallbackContext ctx)
        {
            if (m_PlayerCore.isInteracting) return;

            m_MoveDir = m_Camera.forward * m_PlayerCore.PlayerInput.vertical;
            m_MoveDir += m_Camera.right * m_PlayerCore.PlayerInput.horizontal;

            if (m_PlayerCore.PlayerInput.moveAmount > 0)
            {
                m_PlayerCore.AnimatorController.PlayTargetAnimation("Rolling", true);
                transform.rotation = Quaternion.LookRotation(new Vector3(m_MoveDir.x, 0f, m_MoveDir.z));
            }
            else
            {
                m_PlayerCore.AnimatorController.PlayTargetAnimation("BackStep", true);
            }
        }

        public void HandleFalling(float delta)
        {
            var groundCheckPoint = transform.position + Vector3.up * GroundDetectPointOffset;
            if (Physics.Raycast(groundCheckPoint, Vector3.down, out var hit,
                    GroundDetectMinimumDistance, m_IgnoreLayerMask)) //如果在地面上
            {
                transform.position = hit.point;
                if (m_PlayerCore.isGrounded)
                    return;

                //先检查一下是不是落地
                if (m_FallingTimer <= 0.3f) //0.3秒内的落地不需要播放任何动画
                    m_PlayerCore.AnimatorController.PlayTargetAnimation("Empty", false);
                else if (m_FallingTimer <= 0.7f) //0.7秒内的落地播放小硬直动画
                    m_PlayerCore.AnimatorController.PlayTargetAnimation("Land_Easy", true);
                else
                    m_PlayerCore.AnimatorController.PlayTargetAnimation("Land_Hard", true);

                m_FallingTimer = 0f;
                m_PlayerCore.isGrounded = true;
            }
            else
            {
                if (m_PlayerCore.isGrounded)
                    m_PlayerCore.isGrounded = false;

                if (!m_PlayerCore.isInteracting)
                    m_PlayerCore.AnimatorController.PlayTargetAnimation("Falling", true);

                m_RigidBody.AddForce(Vector3.down * MovementUtility.GetFallingVelocity(m_FallingTimer),
                    ForceMode.VelocityChange);
                m_FallingTimer += delta;
            }
        }
    }
}