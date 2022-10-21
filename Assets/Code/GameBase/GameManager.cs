using System.Collections.Generic;
using UnityEngine;

namespace Code.GameBase
{
    public interface IBaseManager
    {
        void OnStart();
        void Update();
        void OnDispose();
    }

    public abstract class BaseManager : IBaseManager
    {
        public abstract void OnStart();
        protected bool IsComplete { get; set; }

        public virtual void Update()
        {
            
        }

        public abstract void OnDispose();
    }
    
    public class GameManager : MonoBehaviour
    {
        private List<IBaseManager> m_BaseManagers;
        private static GameManager _instance;

        public void Awake()
        {
            m_BaseManagers = new List<IBaseManager>
            {
                new AssetManager(),
            };
            _instance = this;
        }

        public T GetMgr<T>()
        {
            foreach (var manager in m_BaseManagers)
            {
                if (manager is T target)
                    return target;
            }

            Debug.LogError("Cannot find manager");
            return default;
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

        public static AssetManager AssetManager => _instance.GetMgr<AssetManager>();
    }
}