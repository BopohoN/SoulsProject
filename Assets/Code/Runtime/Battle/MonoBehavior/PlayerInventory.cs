using UnityEngine;

namespace Code.Runtime.Battle.MonoBehavior
{
    public class PlayerInventory : MonoBehaviour
    {
        public int rightWeapon;
        public int leftWeapon;
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