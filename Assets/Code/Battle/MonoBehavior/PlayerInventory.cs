using System;
using Code.Battle.Information;
using UnityEngine;

namespace Code.Battle.MonoBehavior
{
    public class PlayerInventory : MonoBehaviour
    {
        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;
        private WeaponSlotManager m_WeaponSlotManager;

        private void Awake()
        {
            m_WeaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        private void Start()
        {
            m_WeaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            m_WeaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
        }
    }
}
