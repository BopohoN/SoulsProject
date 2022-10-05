using Code.GameBase;
using Code.Utility;
using UnityEngine;

namespace Code
{
    public class PlayerController : MonoBehaviour
    {
        private Transform m_Camera;
        private Vector3 m_MoveDir;
        
        [Header("Stats")]
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float rotationSpeed = 10;
        private Rigidbody m_RigidBody;
        private AnimatorController m_AnimatorController;
        private PlayerInputManager m_InputManager;

        private void Start()
        {
            m_RigidBody = GetComponent<Rigidbody>();
            m_AnimatorController = transform.GetChild(0).GetComponent<AnimatorController>();
            m_AnimatorController.Initialize();
            m_InputManager = GameManager.PlayerInputManager;
            m_Camera = Camera.main.transform;
        }

        private Vector3 m_NormalVector;
        private Vector3 m_TargetPosition;

        private void Update()
        {
            m_MoveDir = m_Camera.forward * m_InputManager.Vertical;
            m_MoveDir += m_Camera.right * m_InputManager.Horizontal;
            m_MoveDir.Normalize();

            var speed = movementSpeed;
            var moveAmount = MovementUtility.ClampMovement(m_InputManager.MoveAmount);
            m_MoveDir *= speed * moveAmount;

            var projectedVelocity = Vector3.ProjectOnPlane(new Vector3(m_MoveDir.x, 0, m_MoveDir.z), m_NormalVector);
            m_RigidBody.velocity = projectedVelocity;

            m_AnimatorController.UpdateAnimatorValue(m_InputManager.MoveAmount, 0);
            if (m_AnimatorController.canRotate)
                HandlerRotation(Time.deltaTime);
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

    }
}