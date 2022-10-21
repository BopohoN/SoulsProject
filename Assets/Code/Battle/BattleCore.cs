using System.Collections.Generic;
using Code.Battle.Manager;
using Code.GameBase;
using UnityEngine;

namespace Code.Battle
{
    public interface IBattleManager
    {
        BattleCore Core { get; set; }
        void Init();
        void AddListener();
        void RemoveListener();
        void LogicUpdate();
        void PresentationUpdate();
        void FixedUpdate();
        void Dispose();
    }

    public abstract class BaseBattleManager : IBattleManager
    {
        public BattleCore Core { get; set; }
        public abstract void Init();

        public virtual void AddListener()
        {
            
        }
        public virtual void RemoveListener()
        {
            
        }
        public virtual void LogicUpdate()
        {
            
        }
        public virtual void PresentationUpdate()
        {
            
        }
        public virtual void FixedUpdate()
        {
            
        }
        public abstract void Dispose();
    }
    
    public class BattleCore : MonoBehaviour
    {
        private readonly List<IBattleManager> m_BaseManagers = new();
        public void InitCore()
        {
            InitManager(new PlayerManager());
            InitManager(new CameraManager());
        }

        private void InitManager(IBattleManager mgr)
        {
            m_BaseManagers.Add(mgr);
            mgr.Core = this;
            mgr.Init();
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

        public void OnBattleStart()
        {
            foreach (var baseManager in m_BaseManagers)
                baseManager.AddListener();
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

        private void OnBattleEnd()
        {
            foreach (var baseManager in m_BaseManagers)
            {
                baseManager.RemoveListener();
                baseManager.Dispose();
            }
        }
    }
}