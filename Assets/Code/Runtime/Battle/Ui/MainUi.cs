using UnityEngine;
using UnityEngine.UI;

namespace Code.Runtime.Battle.Ui
{
    public class MainUi : MonoBehaviour
    {
        private Image m_ImgHealthBar;
        private Image m_ImgStaminaBar;

        private void Awake()
        {
            var root = transform.Find("Root");
            m_ImgHealthBar = root.Find("HealthBar/HealthBarFill").GetComponent<Image>();
            m_ImgStaminaBar = root.Find("StaminaBar/StaminaBarFill").GetComponent<Image>();
        }

        public void SetHealthBar(float healthRate)
        {
            m_ImgHealthBar.fillAmount = healthRate;
        }
        public void SetStaminaBar(float staminaRate)
        {
            m_ImgStaminaBar.fillAmount = staminaRate;
        }
    }
}