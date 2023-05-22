using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Configuration;
using Code.Runtime.Battle.Manager;
using Code.Runtime.Battle.Ui;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Code.Runtime.Battle.MonoBehavior.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        public int RightWeapon => rightHandWeaponSlots[currentRightSlotIndex];
        public int LeftWeapon => leftHandWeaponSlots[currentLeftSlotIndex];
        public WeaponSlotManager WeaponSlotManager { get; set; }

        [SerializeField]
        private int[] rightHandWeaponSlots = new int[3];
        [SerializeField]
        private int[] leftHandWeaponSlots = new int[3];

        public List<int> weaponInventory;
        
        public int currentRightSlotIndex = 0;
        public int currentLeftSlotIndex = 0;
        private PlayerCore m_PlayerCore;

        private void Awake()
        {
            WeaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
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
            WeaponSlotManager.LoadWeaponOnSlot(RightWeapon, false);
            WeaponSlotManager.LoadWeaponOnSlot(LeftWeapon, true);

            m_PlayerCore.PlayerInput.OnDRightPressed += ChangeRightWeapon;
            m_PlayerCore.PlayerInput.OnDLeftPressed += ChangeLeftWeapon;
        }

        public void AddWeaponToInventory(int weaponId)
        {
            weaponInventory.Add(weaponId);
        }

        private void ChangeRightWeapon(InputAction.CallbackContext ctx)
        {
            if (rightHandWeaponSlots.All(weaponId => weaponId == 0) ||//玩家没有装备任何武器
                WeaponSlotManager.SwitchingWeapon ||
                m_PlayerCore.isInteracting)//玩家正在交互中
            {
                m_PlayerCore.Core.GetMgr<UiManager>().GetUi<MainUi>(EBattleUi.MainUi)
                    .DoChangeSlotPresentation(MainUi.EQuickSlot.RightWeapon);
                return;
            }
            
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
            m_PlayerCore.AnimatorController.PlayTargetAnimation(
                string.Format(WeaponConfig.D[RightWeapon].OneHandEquipAnimation, "R"), false);
            m_PlayerCore.Core.GetMgr<UiManager>().GetUi<MainUi>(EBattleUi.MainUi)
                .ChangeQuickSlot(RightWeapon, 0, MainUi.EQuickSlot.RightWeapon);
            m_PlayerCore.Core.GetMgr<UiManager>().GetUi<MainUi>(EBattleUi.MainUi)
                .DoChangeSlotPresentation(MainUi.EQuickSlot.RightWeapon);
            
            StartCoroutine(LoadRightWeapon());
        }

        private void ChangeLeftWeapon(InputAction.CallbackContext ctx)
        {
            m_PlayerCore.Core.GetMgr<UiManager>().GetUi<MainUi>(EBattleUi.MainUi)
                .DoChangeSlotPresentation(MainUi.EQuickSlot.LeftWeapon);
            if (m_PlayerCore.isInteracting) //玩家正在交互中
                return;
            if (WeaponSlotManager.SwitchingWeapon)
                return;

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
            
            m_PlayerCore.AnimatorController.PlayTargetAnimation(
                string.Format(WeaponConfig.D[LeftWeapon].OneHandEquipAnimation, "L"), false);
            m_PlayerCore.Core.GetMgr<UiManager>().GetUi<MainUi>(EBattleUi.MainUi)
                .ChangeQuickSlot(RightWeapon, 0, MainUi.EQuickSlot.RightWeapon);
            StartCoroutine(LoadLeftWeapon());
        }
        
        private IEnumerator LoadRightWeapon()
        {
            WeaponSlotManager.SetSwitchingWeapon(true);
            while (!WeaponSlotManager.ReadyToLoadWeapon)
                yield return new WaitForEndOfFrame();
            WeaponSlotManager.LoadWeaponOnSlot(RightWeapon, false);
            yield return new WaitForSeconds(0.5f);
            WeaponSlotManager.SetSwitchingWeapon(false);
        }

        private IEnumerator LoadLeftWeapon()
        {
            WeaponSlotManager.SetSwitchingWeapon(true);
            while (!WeaponSlotManager.ReadyToLoadWeapon)
                yield return new WaitForEndOfFrame();
            WeaponSlotManager.LoadWeaponOnSlot(LeftWeapon, true);
            yield return new WaitForSeconds(0.5f);
            WeaponSlotManager.SetSwitchingWeapon(false);
        }

    }
}