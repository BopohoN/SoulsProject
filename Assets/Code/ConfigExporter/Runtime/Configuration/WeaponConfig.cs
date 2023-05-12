using System;
using System.Collections.Generic;
using System.IO;
using Code.ConfigExporter.Runtime;
using UnityEngine;

//This file is generated by code, do not modify!
namespace Code.Configuration
{
    public class WeaponConfig
    {
		/*
		唯一ID
		*/
		public int Id { get; private set; }
		public string Name { get; private set; }
		public int Atk { get; private set; }
		public string Prefab { get; private set; }
		public string Icon { get; private set; }
		public int BaseStamina { get; private set; }
		public int LightAttackActionId { get; private set; }
		public float LightAttackStaminaMult { get; private set; }
		public int HeavyAttackActionId { get; private set; }
		public float HeavyAttackStaminaMult { get; private set; }
		public string OneHandEquipAnimation { get; private set; }
		public string TwoHandEquipAnimation { get; private set; }

        public static Dictionary<int, WeaponConfig> D = null;
        public const string ConfigName = "WeaponConfig";
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
            D = new Dictionary<int, WeaponConfig>(rows);
            for (var r = 0; r < rows; r++)
            {
                var id = configDeserializer.ReadInt();
                D.Add(id, new WeaponConfig
                {
                    Id = id,
					Name = configDeserializer.ReadString(),
					Atk = configDeserializer.ReadInt(),
					Prefab = configDeserializer.ReadString(),
					Icon = configDeserializer.ReadString(),
					BaseStamina = configDeserializer.ReadInt(),
					LightAttackActionId = configDeserializer.ReadInt(),
					LightAttackStaminaMult = configDeserializer.ReadFloat(),
					HeavyAttackActionId = configDeserializer.ReadInt(),
					HeavyAttackStaminaMult = configDeserializer.ReadFloat(),
					OneHandEquipAnimation = configDeserializer.ReadString(),
					TwoHandEquipAnimation = configDeserializer.ReadString(),

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

