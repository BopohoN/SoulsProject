using Code.Battle.Information;
using UnityEngine;

namespace Code.Battle.MonoBehavior
{
    public class PlayerAttacker : MonoBehaviour
    {
        private PlayerCore m_PlayerCore;

        private void Awake()
        {
            m_PlayerCore = GetComponent<PlayerCore>();
        }

        public void HandleLightAttack()
        {
            m_PlayerCore.AnimatorController.PlayTargetAnimation(
                m_PlayerCore.PlayerInventory.rightWeapon.OH_Light_Attack_1, true);
        }

        public void HandleHeavyAttack()
        {
            m_PlayerCore.AnimatorController.PlayTargetAnimation(
                m_PlayerCore.PlayerInventory.rightWeapon.OH_Heavy_Attack_1, true);
        }
    }
}