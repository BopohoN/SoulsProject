using UnityEngine;

namespace Code.GameBase
{
    public class CameraManager : BaseManager
    {
        private Transform m_Transform;
        private Transform m_TargetTransform;
        private Transform m_CameraTransform;
        private Transform m_CameraPivotTransform;
        private Vector3 m_CameraTransformPosition;
        private LayerMask m_IgnoreLayers;

        public float LookSpeed = 0.1f;
        public float FollowSpeed = 0.1f;
        public float PivotSpeed = 0.03f;

        private Vector3 m_DefaultPosition;
        private float m_LookAngle;
        private float m_PivotAngle;
        public float MinimumPivot = -35f;
        public float MaximumPivot = 35f;
        public override void OnStart()
        {
            m_Transform = GameManager.AssetManager.InitializeObject("CameraHolder").transform;
            m_TargetTransform = GameManager.PlayerManager.Player.transform;
            m_CameraPivotTransform = m_Transform.GetChild(0);
            m_CameraTransform = m_CameraPivotTransform.GetChild(0);
            m_DefaultPosition = new Vector3(0f, 0f, m_CameraTransform.localPosition.z);
            m_IgnoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        }
        
        public override void FixedUpdate()
        {
            var playerInputMgr = GameManager.PlayerInputManager;
            FollowTarget(Time.deltaTime);
            HandleCameraRotation(Time.deltaTime, playerInputMgr.MouseX, playerInputMgr.MouseY);
        }

        private void FollowTarget(float delta)
        {
            var targetPosition = Vector3.Lerp(m_Transform.position, m_TargetTransform.position, delta / FollowSpeed);
            m_Transform.position = targetPosition;
        }

        private void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            if (delta == 0) return;

            m_LookAngle += (mouseXInput * LookSpeed) / delta;
            m_PivotAngle -= (mouseYInput * PivotSpeed) / delta;
            m_PivotAngle = Mathf.Clamp(m_PivotAngle, MinimumPivot, MaximumPivot);

            m_Transform.localRotation = Quaternion.Euler(new Vector3(0f, m_LookAngle, 0f));
            m_CameraPivotTransform.localRotation = Quaternion.Euler(new Vector3(m_PivotAngle, 0f, 0f));
        }

        public override void OnDispose()
        {
            
        }
    }
}