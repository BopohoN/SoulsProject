using Code.Configuration;
using Code.Runtime.Utility;
using UnityEngine;

namespace Code.Runtime.Battle.MonoBehavior
{
    public class PlayerAttacker : MonoBehaviour
    {
        public int lastAttack;
        private PlayerCore m_PlayerCore;

        private void Awake()
        {
            m_PlayerCore = GetComponent<PlayerCore>();
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
                if (isLastAttackLight)
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