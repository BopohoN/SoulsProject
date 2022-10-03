using UnityEngine.AddressableAssets;

namespace Code.GameManager
{
    public class PlayerManager : BaseManager
    {
        public override void OnStart()
        {
            GameManager.AssetManager.InitializeObject("PlayerArmature");
        }

        public override void OnDispose()
        {
            
        }
    }
}