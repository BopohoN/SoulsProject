using Code.Utility;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Battle.MonoBehavior
{
    public class PlayerController : MonoBehaviour
    {
        private float m_FallingTimer;
        private CapsuleCollider m_Collider;
        private Transform m_Camera;
        private Vector3 m_MoveDir;
        private LayerMask m_IgnoreLayerMask;

        private bool m_IsGround;
        
        [Header("Stats")]
        [SerializeField] private float gravity = 9.8f;
        [SerializeField] private float walkSpeed = 1.5f;
        [SerializeField] private float fullMoveSpeed = 4.5f;
        [SerializeField] private float sprintingSpeed = 6f;
        [SerializeField] private float rotationSpeed = 10f;
        private Rigidbody m_RigidBody;
        private AnimatorController m_AnimatorController;
        private PlayerInput m_PlayerInput;

        private bool m_IsSprinting;

        private const float GroundDetectPointOffset = 0.3f;
        private const float GroundDetectMinimumDistance = 0.5f;

        private void Start()
        {
            m_RigidBody = GetComponent<Rigidbody>();
            m_Collider = GetComponent<CapsuleCollider>();
            m_AnimatorController = transform.GetChild(0).GetComponent<AnimatorController>();
            m_AnimatorController.Initialize(OnAnimatorMovement);
            m_PlayerInput = GetComponent<PlayerInput>();
            m_FallingTimer = 0f;
            m_Camera = Camera.main.transform;
            m_IgnoreLayerMask = ~(1 << 9 | 1 << 10);

            m_PlayerInput.OnBPressed += HandleRollingAndSprinting;
            m_PlayerInput.OnBHold += HandleSprint;
            m_PlayerInput.OnBRelease += ResetRollAndSprint;
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
            HandleFalling(Time.deltaTime);
            m_AnimatorController.SetCanRotate(!m_AnimatorController.IsInteractingFlag);
        }

        private void HandlerRotation(float delta)
        {
            var moveOverride = m_PlayerInput.vertical;

            var targetDir = m_Camera.forward * m_PlayerInput.vertical;
            targetDir += m_Camera.right * m_PlayerInput.horizontal;
            
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
            m_MoveDir = m_Camera.forward * m_PlayerInput.vertical;
            m_MoveDir += m_Camera.right * m_PlayerInput.horizontal;
            m_MoveDir.Normalize();

            var moveAmount = MovementUtility.ClampMovement(m_PlayerInput.moveAmount);
            var speed = moveAmount >= 0.8f ? fullMoveSpeed : walkSpeed;
            m_MoveDir *= m_IsSprinting ? sprintingSpeed : speed;

            var projectedVelocity = Vector3.ProjectOnPlane(new Vector3(m_MoveDir.x, 0, m_MoveDir.z), m_NormalVector);
            m_RigidBody.velocity = projectedVelocity;

            m_AnimatorController.UpdateAnimatorValue(m_PlayerInput.moveAmount, 0, m_IsSprinting);
            if (m_AnimatorController.CanRotate)
                HandlerRotation(delta);
        }
        
        private void ResetRollAndSprint(InputAction.CallbackContext ctx)
        {
            m_IsSprinting = false;
        }

        private void HandleSprint(InputAction.CallbackContext ctx)
        {
            if (m_PlayerInput.moveAmount > 0.55f)
                m_IsSprinting = true;
        }

        private void HandleRollingAndSprinting(InputAction.CallbackContext ctx)
        {
            if (m_AnimatorController.IsInteractingFlag) return;

            m_MoveDir = m_Camera.forward * m_PlayerInput.vertical;
            m_MoveDir += m_Camera.right * m_PlayerInput.horizontal;

            if (m_PlayerInput.moveAmount > 0)
            {
                m_AnimatorController.PlayTargetAnimation("Rolling", true);
                transform.rotation = Quaternion.LookRotation(new Vector3(m_MoveDir.x, 0f, m_MoveDir.z));
            }
            else
            {
                m_AnimatorController.PlayTargetAnimation("BackStep", true);
            }
        }

        private void HandleFalling(float delta)
        {
            var groundCheckPoint = transform.position + Vector3.up * GroundDetectPointOffset;
            Debug.DrawLine(groundCheckPoint, groundCheckPoint + Vector3.down * GroundDetectMinimumDistance, Color.red,
                0.2f, false);
            if (Physics.Raycast(groundCheckPoint, Vector3.down, out var hit,
                    GroundDetectMinimumDistance, m_IgnoreLayerMask)) //如果在地面上
            {
                transform.position = hit.point;
                if (m_IsGround)
                    return;
                
                //先检查一下是不是落地
                if (m_FallingTimer <= 0.5f) //0.5秒内的落地不需要播放任何动画
                {
                    m_AnimatorController.PlayTargetAnimation("Locomotion", false);
                }
                else if (m_FallingTimer <=1f) //1.5秒内的落地播放小硬直动画
                {
                    m_AnimatorController.PlayTargetAnimation("Land_Easy", true);
                }
                else
                {
                    m_AnimatorController.PlayTargetAnimation("Land_Hard", true);
                }
                
                Debug.LogWarning("Land ground on: " + hit.point);
                m_Collider.enabled = true;
                m_FallingTimer = 0f;
                m_IsGround = true;
            }
            else
            {
                if (m_IsGround)
                    m_IsGround = false;
                
                if (!m_AnimatorController.IsInteractingFlag)
                {
                    m_AnimatorController.PlayTargetAnimation("Falling", true);
                }

                m_Collider.enabled = false;
                m_RigidBody.AddForce(Vector3.down * MovementUtility.GetFallingVelocity(m_FallingTimer),
                    ForceMode.VelocityChange);
                m_FallingTimer += delta;
            }
        }
        
        private void OnDestroy()
        {
            m_PlayerInput.OnBPressed -= HandleRollingAndSprinting;
        }
    }
}