using System;
using Code.Configuration;
using Code.Runtime.GameBase;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Runtime.Battle.Ui
{
    public class MainUi : MonoBehaviour
    {
        private Image m_ImgHealthBar;
        private Image m_ImgStaminaBar;
        private Transform m_InteractTipsRoot;
        private Transform m_ItemPopRoot;
        private Text m_InteractText;
        private Image m_ImgPopUp;
        private Text m_TextPopUp;

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
    }
}