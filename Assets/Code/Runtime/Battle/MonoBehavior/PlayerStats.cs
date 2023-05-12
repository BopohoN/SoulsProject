using Code.Configuration;
using Code.Runtime.Battle.Manager;
using Code.Runtime.Battle.Ui;
using Code.Runtime.Utility;
using UnityEngine;

namespace Code.Runtime.Battle.MonoBehavior
{
    public class PlayerStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;
        
        public int staminaLevel = 6;
        public int maxStamina;
        public int currentStamina;

        private PlayerCore m_PlayerCore;

        private void Awake()
        {
            m_PlayerCore = GetComponent<PlayerCore>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentHealth = maxHealth;
            currentStamina = maxStamina;
        }

        private int SetMaxHealthFromHealthLevel()
        {
            var health = healthLevel <= ConstConfig.D[CalculateUtility.ConstId.HpSoftMaxLevel].Value
                ? Mathf.FloorToInt(CalculateUtility.CalculateValueFromFormula(CalculateUtility.FormulaId.BaseHp,
                    healthLevel.ToString(), ConstConfig.D[CalculateUtility.ConstId.HpSoftMaxLevel].Value.ToString(),
                    ConstConfig.D[CalculateUtility.ConstId.HpSoftMaxValue].Value.ToString()))
                : Mathf.FloorToInt(Mathf.Lerp(ConstConfig.D[CalculateUtility.ConstId.HpSoftMaxValue].Value,
                    ConstConfig.D[CalculateUtility.ConstId.MaxHp].Value,
                    Mathf.InverseLerp(ConstConfig.D[CalculateUtility.ConstId.HpSoftMaxLevel].Value,
                        ConstConfig.D[CalculateUtility.ConstId.StatusMaxLevel].Value, healthLevel)));
            return health;
        }

        private int SetMaxStaminaFromStaminaLevel()
        {
            var stamina = staminaLevel <= ConstConfig.D[CalculateUtility.ConstId.StaminaSoftMaxLevel].Value
                ? Mathf.FloorToInt(CalculateUtility.CalculateValueFromFormula(CalculateUtility.FormulaId.BaseStamina,
                    staminaLevel.ToString(), ConstConfig.D[CalculateUtility.ConstId.StaminaSoftMaxLevel].Value.ToString(),
                    ConstConfig.D[CalculateUtility.ConstId.StaminaSoftMaxValue].Value.ToString()))
                : Mathf.FloorToInt(Mathf.Lerp(ConstConfig.D[CalculateUtility.ConstId.StaminaSoftMaxValue].Value,
                    ConstConfig.D[CalculateUtility.ConstId.MaxStamina].Value,
                    Mathf.InverseLerp(ConstConfig.D[CalculateUtility.ConstId.StaminaSoftMaxLevel].Value,
                        ConstConfig.D[CalculateUtility.ConstId.StatusMaxLevel].Value, staminaLevel)));
            return stamina;
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

        public void DrainStamina(float stamina)
        {
            var targetStamina = Mathf.FloorToInt(currentStamina - stamina);
            currentStamina = Mathf.Clamp(targetStamina, ConstConfig.D[CalculateUtility.ConstId.MinStamina].Value,
                maxStamina);
            
            var uiManager = m_PlayerCore.Core.GetMgr<UiManager>();
            var battleMainUi = uiManager.GetUi<MainUi>(EBattleUi.MainUi);
            battleMainUi.SetStaminaBar(currentStamina / (float) maxStamina);
        }
    }
}