using InputSystemActions;

namespace Code.GameManager
{
    public class PlayerInputManager:IBaseManager
    {
        public PlayerInputActions PlayerInputActions;

        public void OnStart()
        {
            PlayerInputActions = new PlayerInputActions();
        }

        public void LogicUpdate()
        {
            
        }

        public void PresentationUpdate()
        {
            
        }

        public void FixedUpdate()
        {
            
        }

        public void OnDispose()
        {
            
        }
    }
}