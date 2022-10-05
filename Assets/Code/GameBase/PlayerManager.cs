using UnityEngine;

namespace Code.GameBase
{
    public class PlayerManager : BaseManager
    {
        public GameObject Player { get; private set; }

        public override void OnStart()
        {
            Player = GameManager.AssetManager.InitializeObject("PlayerArmature");
            Player.AddComponent<PlayerController>();
        }

        public override void OnDispose()
        {
            
        }
    }
}