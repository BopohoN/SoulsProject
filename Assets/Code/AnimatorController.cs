using Code.Utility;
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
            var v = MovementUtility.ClampMovement(verticalMovement);
            var h = MovementUtility.ClampMovement(horizontalMovement);
            m_Anim.SetFloat(Vertical, v, 0.1f, Time.deltaTime);
            m_Anim.SetFloat(Horizontal, h, 0.1f, Time.deltaTime);
        }

        public void CanRotate(bool value)
        {
            canRotate = value;
        }
    }
}