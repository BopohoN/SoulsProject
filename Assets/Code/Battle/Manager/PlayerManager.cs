using Code.GameBase;
using UnityEngine;

namespace Code.Battle.Manager
{
    public class PlayerManager: BaseBattleManager
    {
        public GameObject Player { get; private set; }
        public override void Init()
        {
            Player = GameManager.AssetManager.InitializeObject("PlayerArmature");
        }

        public override void AddListener()
        {
            
        }

        public override void Dispose()
        {
            
        }
    }
}