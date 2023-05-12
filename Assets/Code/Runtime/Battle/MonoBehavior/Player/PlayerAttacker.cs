using Code.Configuration;
using Code.Runtime.Utility;
using UnityEngine;

namespace Code.Runtime.Battle.MonoBehavior.Player
{
    public class PlayerAttacker : MonoBehaviour
    {
        public int lastAttack;
        private PlayerCore m_PlayerCore;
        public WeaponSlotManager WeaponSlotManager { get; set; }

        private void Awake()
        {
            WeaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            m_PlayerCore = GetComponent<PlayerCore>();
        }

        private void Start()
        {
            WeaponSlotManager.Init(DoDrainStamina);
        }

        private void DoDrainStamina(bool isHeavy, bool isLeft)
        {
            if (isLeft)
            {
                var currentWeapon = m_PlayerCore.PlayerInventory.LeftWeapon;
                if (isHeavy)
                {
                    m_PlayerCore.PlayerStats.DrainStamina(WeaponConfig.D[currentWeapon].BaseStamina *
                                                          WeaponConfig.D[currentWeapon].HeavyAttackStaminaMult);
                }
                else
                {
                    m_PlayerCore.PlayerStats.DrainStamina(WeaponConfig.D[currentWeapon].BaseStamina *
                                                          WeaponConfig.D[currentWeapon].LightAttackStaminaMult);
                }
            }
            else
            {
                var currentWeapon = m_PlayerCore.PlayerInventory.RightWeapon;
                if (isHeavy)
                {
                    m_PlayerCore.PlayerStats.DrainStamina(WeaponConfig.D[currentWeapon].BaseStamina *
                                                          WeaponConfig.D[currentWeapon].HeavyAttackStaminaMult);
                }
                else
                {
                    m_PlayerCore.PlayerStats.DrainStamina(WeaponConfig.D[currentWeapon].BaseStamina *
                                                          WeaponConfig.D[currentWeapon].LightAttackStaminaMult);
                }
            }
        }


        public void HandleWeaponCombo(int weaponId,bool isLight)
        {
            m_PlayerCore.AnimatorController.SetCanDoCombo(false);
            var isLastAttackLight = WeaponActionUtility.CheckWeaponActionIsLightAttack(lastAttack);
            if ((isLastAttackLight && isLight) || (!isLastAttackLight && !isLight))
            {
                var nextActionId = WeaponActionConfig.D[lastAttack].NextComboId;
                m_PlayerCore.AnimatorController.PlayTargetAnimation(
                    (m_PlayerCore.isOH ? "oh" : "th") + WeaponActionConfig.D[nextActionId].Animation, true);
                lastAttack = nextActionId;
            }
            else
            {
                if (isLight)
                    HandleLightAttack(weaponId);
                else
                    HandleHeavyAttack(weaponId);
            }
        }

        public void HandleLightAttack(int weaponId)
        {
            var actionConfig = WeaponActionConfig.D[WeaponConfig.D[weaponId].LightAttackActionId];
            m_PlayerCore.AnimatorController.PlayTargetAnimation((m_PlayerCore.isOH ? "oh" : "th") + actionConfig.Animation,
                true);
            lastAttack = actionConfig.Id;
        }

        public void HandleHeavyAttack(int weaponId)
        {
            var actionConfig = WeaponActionConfig.D[WeaponConfig.D[weaponId].HeavyAttackActionId];
            m_PlayerCore.AnimatorController.PlayTargetAnimation((m_PlayerCore.isOH ? "oh" : "th") + actionConfig.Animation,
                true);
            lastAttack = actionConfig.Id;
        }
    }
}