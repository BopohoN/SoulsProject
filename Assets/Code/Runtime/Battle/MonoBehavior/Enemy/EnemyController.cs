using System;
using UnityEngine;

namespace Code.Runtime.Battle.MonoBehavior.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        public EnemyStates EnemyStates { get; private set; }
        public EnemyAnimatorController EnemyAnimController { get; private set; }
        private Rigidbody m_RigidBody;

        private void Awake()
        {
            m_RigidBody = GetComponent<Rigidbody>();
            EnemyStates = GetComponent<EnemyStates>();
            EnemyAnimController = transform.GetChild(0).GetComponent<EnemyAnimatorController>();
        }

        private void Start()
        {
            EnemyAnimController.Initialize(OnAnimatorMovement);
        }

        private void OnAnimatorMovement(Animator animator)
        {
            m_RigidBody.drag = 0f;
            var deltaPosition = animator.deltaPosition;
            m_RigidBody.velocity = new Vector3(deltaPosition.x, 0, deltaPosition.z) / Time.deltaTime;
        }

    }
}