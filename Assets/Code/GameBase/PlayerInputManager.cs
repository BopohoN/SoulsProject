using InputSystemActions;
using UnityEngine;

namespace Code.GameBase
{
    public class PlayerInputManager:IBaseManager
    {
        public Vector2 PlayerMovementVector { get; private set; }
        private PlayerInputActions m_PlayerInputActions;

        private float velocityX;
        private float velocityY;

        private const float Attenuation = 0.1f;

        public void OnStart()
        {
            m_PlayerInputActions = new PlayerInputActions();
            m_PlayerInputActions.Player.Enable();
        }

        public void LogicUpdate()
        {
            
        }

        public void PresentationUpdate()
        {
            
        }

        public void FixedUpdate()
        {
            var inputVector = m_PlayerInputActions.Player.Movement.ReadValue<Vector2>();
            var currentX = Mathf.SmoothDamp(PlayerMovementVector.x, inputVector.x, ref velocityX, Attenuation);
            var currentY = Mathf.SmoothDamp(PlayerMovementVector.y, inputVector.y, ref velocityY, Attenuation);
            PlayerMovementVector = new Vector2(currentX, currentY);
        }

        public void OnDispose()
        {
            
        }
    }
}