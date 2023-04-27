﻿/*using System;
using System.Collections.Generic;
using System.IO;
using Code.ConfigExporter.Runtime;
using UnityEngine;

//This file is generated by code, do not modify!
namespace Code.Configuration
{
    public class #CONFIG_NAME#
    {
#PROPERTIES#
        public static Dictionary<int, #CONFIG_NAME#> D = null;
        public const string ConfigName = "#CONFIG_NAME#";
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
            D = new Dictionary<int, #CONFIG_NAME#>(rows);
            for (var r = 0; r < rows; r++)
            {
                var id = configDeserializer.ReadInt();
                D.Add(id, new #CONFIG_NAME#
                {
                    Id = id,
#PROPERTIES_LOAD#
                });
            }
            onLoaded?.Invoke();
        }

        public static void Reset()
        {
            D = null;
        }
    }
}*/
