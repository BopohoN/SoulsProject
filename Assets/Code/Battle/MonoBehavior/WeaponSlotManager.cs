using System;
using Code.Battle.Information;
using UnityEngine;

namespace Code.Battle.MonoBehavior
{
    public class WeaponSlotManager : MonoBehaviour
    {
        private WeaponHolderSlot m_LeftHandSlot;
        private WeaponHolderSlot m_RightHandSlot;

        private void Awake()
        {
            var weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            Debug.Assert(weaponHolderSlots.Length == 2);
            foreach (var slot in weaponHolderSlots)
            {
                if (slot.isLeftHandSlot)
                    m_LeftHandSlot = slot;
                else
                    m_RightHandSlot = slot;
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
                m_LeftHandSlot.LoadWeaponModel(weaponItem);
            else
                m_RightHandSlot.LoadWeaponModel(weaponItem);
        }
    }
}
