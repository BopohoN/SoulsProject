using System;
using Code.Battle.Manager;
using Code.Battle.Ui;
using UnityEngine;

namespace Code.Battle.MonoBehavior
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
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
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

            m_PlayerCore.AnimatorController.PlayTargetAnimation("core_oh_hit_reaction_medium_F_01", true);
        }
    }
}