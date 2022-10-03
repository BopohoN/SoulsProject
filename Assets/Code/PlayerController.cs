using System;
using Code.GameBase;
using UnityEngine;

namespace Code
{
    public class PlayerController : MonoBehaviour
    {
        private Transform m_Model;
        private Animator m_Animator;
        private PlayerInputManager m_PlayerInputMgr;
        private static readonly int Forward = Animator.StringToHash("Forward");

        private void Awake()
        {
            m_Model = transform.GetChild(0);
            m_Animator = m_Model.GetComponent<Animator>();
            m_PlayerInputMgr = GameManager.PlayerInputManager;
        }

        private void Update()
        {
            m_Animator.SetFloat(Forward, m_PlayerInputMgr.PlayerMovementVector.magnitude);
        }
    }
}