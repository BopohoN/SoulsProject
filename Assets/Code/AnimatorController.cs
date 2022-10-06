using System;
using Code.Utility;
using UnityEngine;

namespace Code
{
    public class AnimatorController : MonoBehaviour
    {
        public bool CanRotate { get; private set; }
        public bool IsInteractingFlag => m_Anim.GetBool(IsInteracting);
        private Animator m_Anim;
        private Action<Animator> m_OnAnimatorMoveAction;

        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int IsInteracting = Animator.StringToHash("IsInteracting");

        public void Initialize(Action<Animator> onAnimatorMove)
        {
            m_Anim = transform.GetComponent<Animator>();
            m_OnAnimatorMoveAction = onAnimatorMove;
            CanRotate = true;
        }

        public void UpdateAnimatorValue(float verticalMovement, float horizontalMovement,bool isSprinting)
        {
            var v = MovementUtility.ClampMovement(verticalMovement);
            var h = MovementUtility.ClampMovement(horizontalMovement);
            
            if (isSprinting)
            {
                v = 2;
                h = horizontalMovement;
            }
            
            m_Anim.SetFloat(Vertical, v, 0.1f, Time.deltaTime);
            m_Anim.SetFloat(Horizontal, h, 0.1f, Time.deltaTime);
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            m_Anim.applyRootMotion = isInteracting;
            m_Anim.SetBool(IsInteracting, isInteracting);
            m_Anim.CrossFade(targetAnim, 0.2f);
        }

        public void SetCanRotate(bool value)
        {
            CanRotate = value;
        }

        private void OnAnimatorMove()
        {
            if (!IsInteractingFlag) return;
            m_OnAnimatorMoveAction?.Invoke(m_Anim);
        }
    }
}