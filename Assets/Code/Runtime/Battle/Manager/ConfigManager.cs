using Code.ConfigExporter.Runtime;
using UnityEngine;

namespace Code.Runtime.Battle.Manager
{
    public class ConfigManager : BaseBattleManager
    {
        public override void Init()
        {
            Debug.Log("开始加载配置表");
            var isLoaded = false;
            ConfigInitHandler.LoadAllConfig(() =>
            {
                isLoaded = true;
                Debug.Log("加载配置表完成！");
            });
        }

        public override void Dispose()
        {
            ConfigInitHandler.ResetAllConfig();
        }
    }
}