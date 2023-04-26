using System;
using System.Collections.Generic;
using System.IO;
using Code.ConfigExporter.Runtime;
using UnityEngine;

//This file is generated by code, do not modify!
namespace Code.Configuration
{
    public class WeaponActionConfig
    {
		/*
		唯一ID
		*/
		public int Id { get; private set; }
		public string Animation { get; private set; }
		public int NextComboId { get; private set; }

        public static Dictionary<int, WeaponActionConfig> D = null;
        public const string ConfigName = "WeaponActionConfig";
        public static void Init(ConfigDeserializer configDeserializer, string configDirectory, Action onLoaded)
        {
            var file = configDirectory + ConfigName + ".bytes";
            if (!File.Exists(file))
            {
                Debug.LogError("Fail to load config : " + file);
                onLoaded?.Invoke();
                return;
            }

            var bytes = File.ReadAllBytes(file);
            configDeserializer.Init(bytes);

            var rows = configDeserializer.ReadInt();
            D = new Dictionary<int, WeaponActionConfig>(rows);
            for (var r = 0; r < rows; r++)
            {
                var id = configDeserializer.ReadInt();
                D.Add(id, new WeaponActionConfig
                {
                    Id = id,
					Animation = configDeserializer.ReadString(),
					NextComboId = configDeserializer.ReadInt(),

                });
            }
            onLoaded?.Invoke();
        }

        public static void Reset()
        {
            D = null;
        }
    }
}

