using Code.Battle;
using UnityEngine;

namespace Code
{
    public class BootGame:MonoBehaviour
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