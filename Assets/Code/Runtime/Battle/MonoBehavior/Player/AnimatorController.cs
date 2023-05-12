﻿using System;
using Code.Runtime.Utility;
using UnityEngine;

namespace Code.Runtime.Battle.MonoBehavior.Player
{
    public class AnimatorController : MonoBehaviour
    {
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int DamageX = Animator.StringToHash("DamageX");
        private static readonly int DamageY = Animator.StringToHash("DamageY");
        private static readonly int IsInteracting = Animator.StringToHash("IsInteracting");
        private static readonly int CanDoCombo = Animator.StringToHash("CanDoCombo");
        private Animator m_Anim;
        private Action<Animator> m_OnAnimatorMoveAction;
        public bool CanRotate { get; private set; }
        public bool IsInteractingFlag => m_Anim.GetBool(IsInteracting);

        private void OnAnimatorMove()
        {
            if (!IsInteractingFlag) return;
            m_OnAnimatorMoveAction?.Invoke(m_Anim);
        }

        public void Initialize(Action<Animator> onAnimatorMove)
        {
            m_Anim = transform.GetComponent<Animator>();
            m_OnAnimatorMoveAction = onAnimatorMove;
            CanRotate = true;
        }

        public void UpdateAnimatorValue(float verticalMovement, float horizontalMovement, bool isSprinting)
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

        public void SetDamageDir(Vector2 dir)
        {
            m_Anim.SetFloat(DamageX, dir.x);
            m_Anim.SetFloat(DamageY, dir.y);
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            m_Anim.applyRootMotion = isInteracting;
            m_Anim.SetBool(IsInteracting, isInteracting);
            m_Anim.CrossFade(targetAnim, 0.2f);
        }

        public void SetLayerWeight(string layerName, float weight)
        {
            var index = m_Anim.GetLayerIndex(layerName);
            m_Anim.SetLayerWeight(index,weight);
        }

        public void SetCanDoCombo(bool value)
        {
            m_Anim.SetBool(CanDoCombo, value);
        }

        public bool GetCanDoCombo()
        {
            return m_Anim.GetBool(CanDoCombo);
        }

        public void SetCanRotate(bool value)
        {
            CanRotate = value;
        }

        public void EnableCombo()
        {
            m_Anim.SetBool(CanDoCombo, true);
        }

        public void DisableCombo()
        {
            m_Anim.SetBool(CanDoCombo, false);
        }
    }
}