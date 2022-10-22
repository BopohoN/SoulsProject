using Code.Battle.Information;
using UnityEngine;

namespace Code.Battle.MonoBehavior
{
    public class PlayerAttacker : MonoBehaviour
    {
        private PlayerCore m_PlayerCore;
        public string lastAttack;

        private void Awake()
        {
            m_PlayerCore = GetComponent<PlayerCore>();
        }

        public void HandleWeaponCombo(WeaponItem weaponItem)
        {
            if (m_PlayerCore.PlayerInput.comboFlag)
            {
                m_PlayerCore.AnimatorController.SetCanDoCombo(false);
                if (lastAttack == weaponItem.OH_Light_Attack_1)
                {
                    m_PlayerCore.AnimatorController.PlayTargetAnimation(weaponItem.OH_Light_Attack_2, true);
                    lastAttack = m_PlayerCore.PlayerInventory.rightWeapon.OH_Light_Attack_2;
                }

                if (lastAttack == weaponItem.OH_Light_Attack_2)
                {
                    m_PlayerCore.AnimatorController.PlayTargetAnimation(weaponItem.OH_Light_Attack_1, true);
                    lastAttack = m_PlayerCore.PlayerInventory.rightWeapon.OH_Light_Attack_1;
                }
                if (lastAttack == weaponItem.OH_Heavy_Attack_1)
                {
                    m_PlayerCore.AnimatorController.PlayTargetAnimation(weaponItem.OH_Heavy_Attack_2, true);
                    lastAttack = m_PlayerCore.PlayerInventory.rightWeapon.OH_Heavy_Attack_2;
                }

                if (lastAttack == weaponItem.OH_Heavy_Attack_2)
                {
                    m_PlayerCore.AnimatorController.PlayTargetAnimation(weaponItem.OH_Heavy_Attack_1, true);
                    lastAttack = m_PlayerCore.PlayerInventory.rightWeapon.OH_Heavy_Attack_1;
                }
            }
        }

        public void HandleLightAttack()
        {
            m_PlayerCore.AnimatorController.PlayTargetAnimation(
                m_PlayerCore.PlayerInventory.rightWeapon.OH_Light_Attack_1, true);
            lastAttack = m_PlayerCore.PlayerInventory.rightWeapon.OH_Light_Attack_1;
        }

        public void HandleHeavyAttack()
        {
            m_PlayerCore.AnimatorController.PlayTargetAnimation(
                m_PlayerCore.PlayerInventory.rightWeapon.OH_Heavy_Attack_1, true);
            lastAttack = m_PlayerCore.PlayerInventory.rightWeapon.OH_Heavy_Attack_1;
        }
    }
}