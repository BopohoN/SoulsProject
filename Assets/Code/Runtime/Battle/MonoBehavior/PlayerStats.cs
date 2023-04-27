using Code.Configuration;
using Code.Runtime.Battle.Manager;
using Code.Runtime.Battle.Ui;
using Code.Runtime.Utility;
using Code.Runtime.Utility.PolishExpression;
using UnityEngine;

namespace Code.Runtime.Battle.MonoBehavior
{
    public class PlayerStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        private PlayerCore m_PlayerCore;

        private void Awake()
        {
            m_PlayerCore = GetComponent<PlayerCore>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = Mathf.FloorToInt(CalculateUtility.CalculateValueFromFormula(CalculateUtility.FormulaId.BaseHp,
                healthLevel.ToString(), ConstConfig.D[10101].Value.ToString(), ConstConfig.D[10102].Value.ToString()));
            return maxHealth;
        }

        public void TakeDamage(int damage, Vector2 damageVec)
        {
            Debug.Log("Player take damage: " + damage);
            currentHealth -= damage;

            var uiManager = m_PlayerCore.Core.GetMgr<UiManager>();
            var battleMainUi = uiManager.GetUi<MainUi>(EBattleUi.MainUi);
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                m_PlayerCore.isDead = true;
                battleMainUi.SetHealthBar(0);
                m_PlayerCore.AnimatorController.PlayTargetAnimation("Death", true);
                return;
            }

            battleMainUi.SetHealthBar(currentHealth / (float) maxHealth);

            var damageDir = DamageUtility.CalculateDamageDirection(damageVec);
            m_PlayerCore.AnimatorController.SetDamageDir(damageDir);
            m_PlayerCore.AnimatorController.PlayTargetAnimation("Hit", true);
        }
    }
}