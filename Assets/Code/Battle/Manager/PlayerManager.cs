using Code.Battle.MonoBehavior;
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
            Player.GetComponent<PlayerCore>().SetBattleCore(Core);
        }

        public override void AddListener()
        {
            
        }

        public override void Dispose()
        {
            
        }
    }
}