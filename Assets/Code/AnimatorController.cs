using UnityEngine;

namespace Code
{
    public class AnimatorController : MonoBehaviour
    {
        public bool canRotate;
        private Animator m_Anim;

        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");

        public void Initialize()
        {
            m_Anim = transform.GetComponent<Animator>();
            
        }

        public void UpdateAnimatorValue(float verticalMovement, float horizontalMovement)
        {
            #region Vertical

            float v;

            if (verticalMovement > 0 && verticalMovement<0.55f)
            {
                v = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                v = 1;
            }
            else if (verticalMovement < 0 & verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1;
            }
            else
            {
                v = 0;
            }
            #endregion
            
            #region Horizontal

            float h;

            if (horizontalMovement > 0 && horizontalMovement<0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 & horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }
            else
            {
                h = 0;
            }
            #endregion

            m_Anim.SetFloat(Vertical, v, 0.1f, Time.deltaTime);
            m_Anim.SetFloat(Horizontal, h, 0.1f, Time.deltaTime);
        }

        public void CanRotate(bool value)
        {
            canRotate = value;
        }
    }
}