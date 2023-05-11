using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Code.Runtime.Battle.MonoBehavior
{
    public class PlayerInventory : MonoBehaviour
    {
        public int RightWeapon => rightHandWeaponSlots[currentRightSlotIndex];
        public int LeftWeapon => leftHandWeaponSlots[currentLeftSlotIndex];
        private WeaponSlotManager m_WeaponSlotManager;

        [SerializeField]
        private int[] rightHandWeaponSlots = new int[3];
        [SerializeField]
        private int[] leftHandWeaponSlots = new int[3];

        public int currentRightSlotIndex = 0;
        public int currentLeftSlotIndex = 0;
        private PlayerCore m_PlayerCore;

        private void Awake()
        {
            m_WeaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            m_PlayerCore = GetComponent<PlayerCore>();
        }

        private void Start()
        {
            rightHandWeaponSlots[0] = 0;
            rightHandWeaponSlots[1] = 30001;
            rightHandWeaponSlots[2] = 0;
            leftHandWeaponSlots[0] = 0;
            leftHandWeaponSlots[1] = 0;
            leftHandWeaponSlots[2] = 0;
            m_WeaponSlotManager.LoadWeaponOnSlot(RightWeapon, false);
            m_WeaponSlotManager.LoadWeaponOnSlot(LeftWeapon, true);

            m_PlayerCore.PlayerInput.OnDRightPressed += ChangeRightWeapon;
            m_PlayerCore.PlayerInput.OnDLeftPressed += ChangeLeftWeapon;
        }

        private void ChangeRightWeapon(InputAction.CallbackContext ctx)
        {
            if (rightHandWeaponSlots.All(weaponId => weaponId == 0)) //玩家没有装备任何武器
                return;
            
            var currentWeaponId = rightHandWeaponSlots[currentRightSlotIndex];
            if (currentWeaponId == 0) //如果玩家目前是空手状态
            {
                //那么就调过所有的空槽位切换成下一个武器
                if (currentRightSlotIndex + 1 > rightHandWeaponSlots.Length -1)
                {
                    var nextSlotIndex = 0;
                    while (rightHandWeaponSlots[nextSlotIndex] == currentWeaponId)
                        nextSlotIndex++;
                    currentRightSlotIndex = nextSlotIndex;
                }
                else
                {
                    do
                    {
                        currentRightSlotIndex++;
                    } while (rightHandWeaponSlots[currentRightSlotIndex] == currentWeaponId);
                }
            }
            else //如果玩家目前有武器，那么就切换成下一个槽位
            {
                if (currentRightSlotIndex + 1 >= rightHandWeaponSlots.Length)
                    currentRightSlotIndex = 0;
                else
                    currentRightSlotIndex++;
            }
            
            m_WeaponSlotManager.LoadWeaponOnSlot(RightWeapon, false);
        }

        private void ChangeLeftWeapon(InputAction.CallbackContext ctx)
        {
            if (leftHandWeaponSlots.All(weaponId => weaponId == 0)) //玩家没有装备任何武器
                return;
            
            var currentWeaponId = leftHandWeaponSlots[currentLeftSlotIndex];
            if (currentWeaponId == 0) //如果玩家目前是空手状态
            {
                //那么就调过所有的空槽位切换成下一个武器
                if (currentLeftSlotIndex + 1 > leftHandWeaponSlots.Length -1)
                {
                    var nextSlotIndex = 0;
                    while (leftHandWeaponSlots[nextSlotIndex] == currentWeaponId)
                        nextSlotIndex++;
                    currentLeftSlotIndex = nextSlotIndex;
                }
                else
                {
                    do
                    {
                        currentLeftSlotIndex++;
                    } while (leftHandWeaponSlots[currentLeftSlotIndex] == currentWeaponId);
                }
            }
            else //如果玩家目前有武器，那么就切换成下一个槽位
            {
                if (currentLeftSlotIndex + 1 >= leftHandWeaponSlots.Length)
                    currentLeftSlotIndex = 0;
                else
                    currentLeftSlotIndex++;
            }
            
            m_WeaponSlotManager.LoadWeaponOnSlot(LeftWeapon, true);
        }
    }
}