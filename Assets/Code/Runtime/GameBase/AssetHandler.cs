using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Runtime.GameBase
{
    public static class AssetHandler
    {
        public static async void InitAddressable(Action onDone)
        {
            var handle = Addressables.InitializeAsync(true);
            await handle.Task;
            onDone?.Invoke();
        }

        public static async void InitializeObjectAsync(string resKey, Action<GameObject> onDone)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(resKey);
            await handle.Task;
            Addressables.Release(handle);
            onDone?.Invoke(handle.Result);
        }

        public static async void LoadAssetsAsync<T>(string resKey, Action<T> onDone)
        {
            var handle = Addressables.LoadAssetAsync<T>(resKey);
            await handle.Task;
            Addressables.Release(handle);
            onDone?.Invoke(handle.Result);
        }

        public static T LoadAssets<T>(string resKey)
        {
            var handle = Addressables.LoadAssetAsync<T>(resKey);
            var ret = handle.WaitForCompletion();
            Addressables.Release(handle);
            return ret;
        }

        public static GameObject InitializeObject(string resKey)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(resKey);
            var ret = handle.WaitForCompletion();
            Addressables.Release(handle);
            return ret;
        }
    }
}