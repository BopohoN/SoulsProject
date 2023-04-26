using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Runtime.GameBase
{
    public class AssetManager : BaseManager
    {
        public override void OnStart()
        {
            AssetHandler.InitAddressable(() => { IsComplete = true; });
        }

        public void InitializeObjectAsync(string resKey, Action<GameObject> onDone)
        {
            AssetHandler.InitializeObjectAsync(resKey, onDone);
        }

        public void LoadAssetsAsync<T>(string resKey, Action<T> onDone)
        {
            AssetHandler.LoadAssetsAsync(resKey, onDone);
        }

        public T LoadAssets<T>(string resKey)
        {
            return AssetHandler.LoadAssets<T>(resKey);
        }

        public GameObject InitializeObject(string resKey,Transform parent)
        {
            var ret = AssetHandler.InitializeObject(resKey);
            var go = Object.Instantiate(ret, parent);
            return go;
        }

        public GameObject InitializeObject(string resKey)
        {
            var ret = AssetHandler.InitializeObject(resKey);
            var go = Object.Instantiate(ret);
            return go;
        }

        public override void OnDispose()
        {
        }
    }
}