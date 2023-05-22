using System;
using System.Collections;
using System.Collections.Generic;
using Code.Configuration;
using Code.Runtime.GameBase;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Runtime.Battle.Ui
{
    public class MainUi : MonoBehaviour
    {
        public enum EQuickSlot
        {
            Magic = 0,
            LeftWeapon = 1,
            RightWeapon = 2,
            Equipment = 3,
        }

        private class Slot
        {
            public Image Icon;
            public Image Flash;
            public TextMeshProUGUI TxtName;
            public TextMeshProUGUI TxtCount;
        }
        
        private Image m_ImgHealthBar;
        private Image m_ImgStaminaBar;
        private Transform m_InteractTipsRoot;
        private Transform m_ItemPopRoot;
        private Text m_InteractText;
        private Image m_ImgPopUp;
        private Text m_TextPopUp;
        private Dictionary<EQuickSlot, Slot> m_QuickSlot;

        private void Awake()
        {
            var root = transform.Find("Root");
            m_ImgHealthBar = root.Find("HealthBar/HealthBarFill").GetComponent<Image>();
            m_ImgStaminaBar = root.Find("StaminaBar/StaminaBarFill").GetComponent<Image>();
            m_InteractTipsRoot = root.Find("InteractTips");
            m_ItemPopRoot = root.Find("ItemPop");
            m_InteractText = m_InteractTipsRoot.Find("Bg/InteractText").GetComponent<Text>();
            m_ImgPopUp = m_ItemPopRoot.Find("Bg/ItemIcon").GetComponent<Image>();
            m_TextPopUp = m_ItemPopRoot.Find("Bg/ItemName").GetComponent<Text>();

            m_QuickSlot = new Dictionary<EQuickSlot, Slot>(4);
            var quickSlotRoot = root.Find("QuickSlot");
            for (var i = 0; i < quickSlotRoot.childCount; i++)
            {
                var slot = quickSlotRoot.GetChild(i);
                m_QuickSlot.Add((EQuickSlot) i, new Slot
                {
                    Icon = slot.Find("Icon").GetComponent<Image>(),
                    Flash = slot.Find("Flash").GetComponent<Image>(),
                    TxtName = slot.Find("TxtName")?.GetComponent<TextMeshProUGUI>(),
                    TxtCount = slot.Find("TxtCount")?.GetComponent<TextMeshProUGUI>()
                });
            }
        }

        private void Start()
        {
            ChangeQuickSlot(0, 0,EQuickSlot.LeftWeapon);
            ChangeQuickSlot(0, 0,EQuickSlot.RightWeapon);
            ChangeQuickSlot(0, 0,EQuickSlot.Magic);
            ChangeQuickSlot(0, 0,EQuickSlot.Equipment);
        }

        public void SetHealthBar(float healthRate)
        {
            m_ImgHealthBar.fillAmount = healthRate;
        }
        public void SetStaminaBar(float staminaRate)
        {
            m_ImgStaminaBar.fillAmount = staminaRate;
        }

        public void SetInteractTipsActive(bool value)
        {
            m_InteractTipsRoot.gameObject.SetActive(value);
        }
        public void SetItemPopupActive(bool value)
        {
            m_ItemPopRoot.gameObject.SetActive(value);
        }
        public void SetInteractTipsText(string text)
        {
            m_InteractText.text = text;
        }
        public void SetItemPopup(int weaponId)
        {
            m_ImgPopUp.sprite = GameManager.AssetManager.LoadAssets<Sprite>(WeaponConfig.D[weaponId].Icon);
            m_TextPopUp.text = WeaponConfig.D[weaponId].Name;
        }

        public void DoChangeSlotPresentation(EQuickSlot quickSlot)
        {
            var slot = m_QuickSlot[quickSlot];
            var color = slot.Flash.color;
            slot.Flash.color = new Color(color.r, color.g, color.b, 0);
            var seq = DOTween.Sequence();
            seq.Append(slot.Flash.DOFade(0.12f, 0.1f));
            seq.Append(slot.Flash.DOFade(0f, 0.1f));
        }

        public void ChangeQuickSlot(int weaponId, int count, EQuickSlot quickSlot)
        {
            var slot = m_QuickSlot[quickSlot];
            slot.Icon.sprite = GameManager.AssetManager.LoadAssets<Sprite>(WeaponConfig.D[weaponId].Icon);
            if (slot.TxtCount != null)
            {
                if (weaponId <= 0 )
                    slot.TxtCount.gameObject.SetActive(false);
                slot.TxtCount.text = count.ToString();
            }
            if (slot.TxtName != null) slot.TxtName.text = WeaponConfig.D[weaponId].Name;
        }
    }
}