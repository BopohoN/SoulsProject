using System;
using System.Collections.Generic;
using Code.Configuration;
using UnityEngine;

namespace Code.ConfigExporter.Runtime
{
    public static class ConfigInitHandler
    {
        private static readonly List<Action<ConfigDeserializer, string, Action>> ConfigInitHandlers =
            new List<Action<ConfigDeserializer, string, Action>>
            {
//INIT_CODE_GENERATE_START
				WeaponActionConfig.Init,
				WeaponConfig.Init,
				ItemConfig.Init,
//INIT_CODE_GENERATE_END
            };

        private static readonly List<Action> ConfigResetHandlers = new List<Action>()
        {
//RESET_CODE_GENERATE_START
				WeaponActionConfig.Reset,
				WeaponConfig.Reset,
				ItemConfig.Reset,
//RESET_CODE_GENERATE_END
        };

        public static void LoadAllConfig(Action onDone)
        {
            var path = Application.dataPath + "/Config/";
            var cd = new ConfigDeserializer();
            var index = -1;

            void Next()
            {
                index++;
                if (index > ConfigInitHandlers.Count - 1)
                {
                    onDone?.Invoke();
                    return;
                }

                ConfigInitHandlers[index](cd, path, Next);
            }

            Next();
        }

        public static void ResetAllConfig()
        {
            foreach (var reset in ConfigResetHandlers)
                reset();
        }
    }
}