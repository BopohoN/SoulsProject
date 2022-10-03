using UnityEngine;

namespace Code.GameBase
{
    public class PlayerManager : BaseManager
    {
        private GameObject m_Player;

        public override void OnStart()
        {
            m_Player = GameManager.AssetManager.InitializeObject("PlayerArmature");
            m_Player.AddComponent<PlayerController>();
        }

        public override void OnDispose()
        {
            
        }
    }
}