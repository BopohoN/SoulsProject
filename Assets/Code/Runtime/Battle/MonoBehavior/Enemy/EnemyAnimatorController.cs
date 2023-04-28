using System;
using UnityEngine;

namespace Code.Runtime.Battle.MonoBehavior.Enemy
{
    public class EnemyAnimatorController : MonoBehaviour
    {
        private Action<Animator> m_OnAnimatorMoveAction;
        private Animator m_Anim;
        public bool IsInteractingFlag => m_Anim.GetBool(IsInteracting);

        private static readonly int IsInteracting = Animator.StringToHash("IsInteracting");
        private static readonly int DamageX = Animator.StringToHash("DamageX");
        private static readonly int DamageY = Animator.StringToHash("DamageY");
        private void Awake()
        {
            m_Anim = GetComponent<Animator>();
        }
        
        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            m_Anim.applyRootMotion = isInteracting;
            m_Anim.SetBool(IsInteracting, isInteracting);
            m_Anim.CrossFade(targetAnim, 0.2f);
        }

        public void SetHitParam(Vector2 dir)
        {
            m_Anim.SetFloat(DamageX, dir.x);
            m_Anim.SetFloat(DamageY, dir.y);
        }
        
        private void OnAnimatorMove()
        {
            if (!IsInteractingFlag) return;
            m_OnAnimatorMoveAction?.Invoke(m_Anim);
        }

        public void Initialize(Action<Animator> onAnimatorMove)
        {
            m_Anim = transform.GetComponent<Animator>();
            m_OnAnimatorMoveAction = onAnimatorMove;
        }

    }
}