using Code.Battle.MonoBehavior;
using Code.GameBase;
using UnityEngine;

namespace Code.Battle.Manager
{
    public class CameraManager : BaseBattleManager
    {
        private Transform m_FocusTransform;
        private Transform m_CameraHolderTransform;
        private Transform m_CameraPivotTransform;
        private Transform m_CameraTransform;
        private Vector3 m_CameraTransformPosition;
        private LayerMask m_IgnoreLayers;
        private Vector3 m_CameraFollowVelocity = Vector3.zero;


        private float m_TargetCameraZoom;
        private float m_DefaultCameraZoom;
        private float m_LookAngle;
        private float m_PivotAngle;

        private const float LookSpeed = 0.1f;
        private const float FollowSpeed = 0.1f;
        private const float CollisionBumpSpeed = 0.1f;
        private const float PivotSpeed = 0.03f;
        
        private const float MinimumPivot = -35f;
        private const float MaximumPivot = 35f;
        
        private const float CameraSphereRadius = 0.2f;
        private const float CameraCollisionOffset = 0.2f;
        private const float CameraMinimumCollisionOffset = 0.2f;
        public override void Init()
        {
            m_CameraHolderTransform = GameManager.AssetManager.InitializeObject("CameraHolder").transform;
            m_FocusTransform = Core.GetMgr<PlayerManager>().Player.transform;
            m_CameraPivotTransform = m_CameraHolderTransform.GetChild(0);
            m_CameraTransform = m_CameraPivotTransform.GetChild(0);
            m_DefaultCameraZoom = m_CameraTransform.localPosition.z;
            m_IgnoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        }

        public override void FixedUpdate()
        {
            var playerInputMgr = Core.GetMgr<PlayerManager>().Player.GetComponent<PlayerInput>();
            FollowTarget(Time.deltaTime);
            HandleCameraRotation(Time.deltaTime, playerInputMgr.mouseX, playerInputMgr.mouseY);
        }

        private void FollowTarget(float deltaTime)
        {
            var targetPosition = Vector3.SmoothDamp(m_CameraHolderTransform.position, m_FocusTransform.position,
                ref m_CameraFollowVelocity, deltaTime / FollowSpeed);
            m_CameraHolderTransform.position = targetPosition;

            HandleCameraCollisions(deltaTime);
        }

        private void HandleCameraRotation(float deltaTime, float mouseXInput, float mouseYInput)
        {
            if (deltaTime == 0) return;

            m_LookAngle += (mouseXInput * LookSpeed) / deltaTime;
            m_PivotAngle -= (mouseYInput * PivotSpeed) / deltaTime;
            m_PivotAngle = Mathf.Clamp(m_PivotAngle, MinimumPivot, MaximumPivot);

            m_CameraHolderTransform.localRotation = Quaternion.Euler(new Vector3(0f, m_LookAngle, 0f));
            m_CameraPivotTransform.localRotation = Quaternion.Euler(new Vector3(m_PivotAngle, 0f, 0f));
        }

        private void HandleCameraCollisions(float deltaTime)
        {
            m_TargetCameraZoom = m_DefaultCameraZoom;
            var direction = m_CameraTransform.position - m_CameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(m_CameraPivotTransform.position, CameraSphereRadius, direction, out var hit,
                    Mathf.Abs(m_TargetCameraZoom), m_IgnoreLayers))
            {
                var dis = Vector3.Distance(m_CameraPivotTransform.position, hit.point);
                m_TargetCameraZoom = -(dis - CameraCollisionOffset);
            }

            if (Mathf.Abs(m_TargetCameraZoom) < CameraMinimumCollisionOffset)
            {
                m_TargetCameraZoom = -CameraMinimumCollisionOffset;
            }

            m_CameraTransformPosition.z =
                Mathf.Lerp(m_CameraTransform.localPosition.z, m_TargetCameraZoom, deltaTime / CollisionBumpSpeed);
            m_CameraTransform.localPosition = m_CameraTransformPosition;
        }

        public override void Dispose()
        {
            
        }
    }
}