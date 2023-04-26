using Code.Runtime.Battle;
using UnityEngine;

namespace Code.Runtime
{
    public class BootGame : MonoBehaviour
    {
        private void Start()
        {
            var go = new GameObject("BattleCore");
            var core = go.AddComponent<BattleCore>();
            core.InitCore();
            core.OnBattleStart();
        }
    }
}