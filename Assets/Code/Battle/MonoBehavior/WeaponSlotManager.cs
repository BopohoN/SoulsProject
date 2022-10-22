using System;
using Code.Battle.Information;
using UnityEngine;

namespace Code.Battle.MonoBehavior
{
    public class WeaponSlotManager : MonoBehaviour
    {
        private WeaponHolderSlot m_LeftHandSlot;
        private WeaponHolderSlot m_RightHandSlot;

        private DamageCollider m_LeftHandDamageCollider; 
        private DamageCollider m_RightHandDamageCollider; 

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
            {
                Debug.Log("LoadWeaponModel m_LeftHandSlot");
                m_LeftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
            }
            else
            {
                Debug.Log("LoadWeaponModel m_RightHandSlot");
                m_RightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
            }
        }

        private void LoadLeftWeaponDamageCollider()
        {
            m_LeftHandDamageCollider = m_LeftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            Debug.Log(m_LeftHandDamageCollider.name);
        }
        private void LoadRightWeaponDamageCollider()
        {
            m_RightHandDamageCollider = m_RightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            Debug.Log(m_RightHandDamageCollider.name);
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
