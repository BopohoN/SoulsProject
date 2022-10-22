using UnityEngine;
using UnityEngine.UI;

namespace Code.Battle.Ui
{
    public class MainUi : MonoBehaviour
    {
        private Image m_ImgHealthBar;

        private void Awake()
        {
            var root = transform.Find("Root");
            m_ImgHealthBar = root.Find("HealthBar/HealthBarFill").GetComponent<Image>();
        }

        public void SetHealthBar(float healthRate)
        {
            m_ImgHealthBar.fillAmount = healthRate;
        }
    }
}
