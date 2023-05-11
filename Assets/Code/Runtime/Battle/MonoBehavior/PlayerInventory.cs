using System.Linq;
using UnityEngine;

namespace Code.Runtime.Battle.MonoBehavior
{
    public class PlayerInventory : MonoBehaviour
    {
        public int RightWeapon => m_RightHandWeaponSlots[currentRightSlotIndex];
        public int LeftWeapon => m_LeftHandWeaponSlots[currentLeftSlotIndex];
        private WeaponSlotManager m_WeaponSlotManager;

        [SerializeField]
        private int[] m_RightHandWeaponSlots = new int[3];
        [SerializeField]
        private int[] m_LeftHandWeaponSlots = new int[3];

        public int currentRightSlotIndex = 0;
        public int currentLeftSlotIndex = 0;

        private void Awake()
        {
            m_WeaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        private void Start()
        {
            m_RightHandWeaponSlots[0] = 0;
            m_RightHandWeaponSlots[1] = 30001;
            m_RightHandWeaponSlots[2] = 0;
            m_LeftHandWeaponSlots[0] = 0;
            m_LeftHandWeaponSlots[1] = 0;
            m_LeftHandWeaponSlots[2] = 0;
            m_WeaponSlotManager.LoadWeaponOnSlot(RightWeapon, false);
            m_WeaponSlotManager.LoadWeaponOnSlot(LeftWeapon, true);
        }

        public void ChangeRightWeapon()
        {
            if (m_RightHandWeaponSlots.All(weaponId => weaponId == 0)) //玩家没有装备任何武器
                return;
            
            var currentWeaponId = m_RightHandWeaponSlots[currentRightSlotIndex];
            if (currentWeaponId == 0) //如果玩家目前是空手状态
            {
                //那么就调过所有的空槽位切换成下一个武器
                if (currentRightSlotIndex + 1 > m_RightHandWeaponSlots.Length -1)
                {
                    var nextSlotIndex = 0;
                    while (m_RightHandWeaponSlots[nextSlotIndex] == currentWeaponId)
                        nextSlotIndex++;
                    currentRightSlotIndex = nextSlotIndex;
                }
                else
                {
                    do
                    {
                        currentRightSlotIndex++;
                    } while (m_RightHandWeaponSlots[currentRightSlotIndex] == currentWeaponId);
                }
            }
            else //如果玩家目前有武器，那么就切换成下一个槽位
            {
                if (currentRightSlotIndex + 1 >= m_RightHandWeaponSlots.Length)
                    currentRightSlotIndex = 0;
                else
                    currentRightSlotIndex++;
            }
            
            m_WeaponSlotManager.LoadWeaponOnSlot(RightWeapon, false);
        }

        public void ChangeLeftWeapon()
        {
            if (m_LeftHandWeaponSlots.All(weaponId => weaponId == 0)) //玩家没有装备任何武器
                return;
            
            var currentWeaponId = m_LeftHandWeaponSlots[currentLeftSlotIndex];
            if (currentWeaponId == 0) //如果玩家目前是空手状态
            {
                //那么就调过所有的空槽位切换成下一个武器
                if (currentLeftSlotIndex + 1 > m_LeftHandWeaponSlots.Length -1)
                {
                    var nextSlotIndex = 0;
                    while (m_LeftHandWeaponSlots[nextSlotIndex] == currentWeaponId)
                        nextSlotIndex++;
                    currentLeftSlotIndex = nextSlotIndex;
                }
                else
                {
                    do
                    {
                        currentLeftSlotIndex++;
                    } while (m_LeftHandWeaponSlots[currentLeftSlotIndex] == currentWeaponId);
                }
            }
            else //如果玩家目前有武器，那么就切换成下一个槽位
            {
                if (currentLeftSlotIndex + 1 >= m_LeftHandWeaponSlots.Length)
                    currentLeftSlotIndex = 0;
                else
                    currentLeftSlotIndex++;
            }
            
            m_WeaponSlotManager.LoadWeaponOnSlot(LeftWeapon, true);
        }
    }
}