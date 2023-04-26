using System.Collections.Generic;
using UnityEngine;

namespace Code.Runtime.GameBase
{
    public interface IBaseManager
    {
        void OnStart();
        void Update();
        void OnDispose();
    }

    public abstract class BaseManager : IBaseManager
    {
        protected bool IsComplete { get; set; }
        public abstract void OnStart();

        public virtual void Update()
        {
        }

        public abstract void OnDispose();
    }

    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        private List<IBaseManager> m_BaseManagers;

        public static AssetManager AssetManager => _instance.GetMgr<AssetManager>();

        public void Awake()
        {
            m_BaseManagers = new List<IBaseManager>
            {
                new AssetManager()
            };
            _instance = this;
        }

        private void Start()
        {
            foreach (var baseManager in m_BaseManagers)
                baseManager.OnStart();
        }

        private void Update()
        {
            foreach (var baseManager in m_BaseManagers)
                baseManager.Update();
        }

        private void OnDestroy()
        {
            foreach (var baseManager in m_BaseManagers)
                baseManager.OnDispose();
        }

        public T GetMgr<T>()
        {
            foreach (var manager in m_BaseManagers)
                if (manager is T target)
                    return target;

            Debug.LogError("Cannot find manager");
            return default;
        }
    }
}