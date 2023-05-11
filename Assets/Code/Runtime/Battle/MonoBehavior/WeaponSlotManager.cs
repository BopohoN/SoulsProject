using Code.Configuration;
using UnityEngine;

namespace Code.Runtime.Battle.MonoBehavior
{
    public class WeaponSlotManager : MonoBehaviour
    {
        private DamageCollider m_LeftHandDamageCollider;
        private WeaponHolderSlot m_LeftHandSlot;
        private DamageCollider m_RightHandDamageCollider;
        private WeaponHolderSlot m_RightHandSlot;

        private void Awake()
        {
            var weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            Debug.Assert(weaponHolderSlots.Length == 2);
            foreach (var slot in weaponHolderSlots)
                if (slot.isLeftHandSlot)
                    m_LeftHandSlot = slot;
                else
                    m_RightHandSlot = slot;
        }

        public void LoadWeaponOnSlot(int weaponId, bool isLeft)
        {
            if (isLeft)
            {
                m_LeftHandSlot.LoadWeaponModel(weaponId);
                LoadLeftWeaponDamageCollider(weaponId);
            }
            else
            {
                m_RightHandSlot.LoadWeaponModel(weaponId);
                LoadRightWeaponDamageCollider(weaponId);
            }
        }

        private void LoadLeftWeaponDamageCollider(int weaponId)
        {
            m_LeftHandDamageCollider = m_LeftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            m_LeftHandDamageCollider.SetWeaponAtk(WeaponConfig.D[weaponId].Atk);
        }

        private void LoadRightWeaponDamageCollider(int weaponId)
        {
            m_RightHandDamageCollider = m_RightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            m_RightHandDamageCollider.SetWeaponAtk(WeaponConfig.D[weaponId].Atk);
        }

        //Animation Events
        public void OpenRightDamageCollider()
        {
            m_RightHandDamageCollider.EnableDamageCollider();
        }

        //Animation Events
        public void OpenLeftDamageCollider()
        {
            m_LeftHandDamageCollider.EnableDamageCollider();
        }

        //Animation Events
        public void CloseRightDamageCollider()
        {
            m_RightHandDamageCollider.DisableDamageCollider();
        }

        //Animation Events
        public void CloseLeftDamageCollider()
        {
            m_LeftHandDamageCollider.DisableDamageCollider();
        }
    }
}