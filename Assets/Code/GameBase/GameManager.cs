using System.Collections.Generic;
using UnityEngine;

namespace Code.GameBase
{
    public interface IBaseManager
    {
        void OnStart();
        void LogicUpdate();
        void PresentationUpdate();
        void FixedUpdate();
        void OnDispose();
    }

    public abstract class BaseManager : IBaseManager
    {
        public abstract void OnStart();
        protected bool IsComplete { get; set; }

        public virtual void LogicUpdate()
        {
            
        }

        public virtual void PresentationUpdate()
        {
            
        }

        public virtual void FixedUpdate()
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
                new PlayerInputManager(),
                new PlayerManager(),
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
                baseManager.LogicUpdate();
            foreach (var baseManager in m_BaseManagers)
                baseManager.PresentationUpdate();
        }

        private void FixedUpdate()
        {
            foreach (var baseManager in m_BaseManagers)
                baseManager.FixedUpdate();
        }

        private void OnDestroy()
        {
            foreach (var baseManager in m_BaseManagers)
                baseManager.OnDispose();
        }

        public static PlayerInputManager PlayerInputManager => _instance.GetMgr<PlayerInputManager>();
        public static AssetManager AssetManager => _instance.GetMgr<AssetManager>();
        public static PlayerManager PlayerManager => _instance.GetMgr<PlayerManager>();
    }
}