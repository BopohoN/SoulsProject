using System.Collections.Generic;
using Code.Runtime.GameBase;
using UnityEngine;

namespace Code.Runtime.Battle.Manager
{
    public enum EBattleUi
    {
        None,
        MainUi
    }

    public interface IBattleUi
    {
    }

    public class UiManager : BaseBattleManager
    {
        private readonly Dictionary<EBattleUi, GameObject> m_Uis = new();

        public override void Init()
        {
            var mainUi = GameManager.AssetManager.InitializeObject("MainUi");
            m_Uis.Add(EBattleUi.MainUi, mainUi);
        }

        public T GetUi<T>(EBattleUi ui)
        {
            return m_Uis[ui].GetComponent<T>();
        }

        public override void Dispose()
        {
        }
    }
}