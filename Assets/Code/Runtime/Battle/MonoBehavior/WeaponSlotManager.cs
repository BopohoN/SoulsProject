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
                if (weaponId >= 0)
                    LoadLeftWeaponDamageCollider();
            }
            else
            {
                m_RightHandSlot.LoadWeaponModel(weaponId);
                if (weaponId >= 0)
                    LoadRightWeaponDamageCollider();
            }
        }

        private void LoadLeftWeaponDamageCollider()
        {
            m_LeftHandDamageCollider = m_LeftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        private void LoadRightWeaponDamageCollider()
        {
            m_RightHandDamageCollider = m_RightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
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