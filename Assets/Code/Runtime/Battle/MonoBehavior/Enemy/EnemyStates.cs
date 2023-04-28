using Code.Runtime.Utility;
using UnityEngine;

namespace Code.Runtime.Battle.MonoBehavior.Enemy
{
    public class EnemyStates : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;
        private EnemyController m_EnemyController;

        private void Awake()
        {
            m_EnemyController = GetComponentInChildren<EnemyController>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage, Vector2 damageVec)
        {
            Debug.Log("Player take damage: " + damage);
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                m_EnemyController.EnemyAnimController.PlayTargetAnimation("Death", true);
                return;
            }

            var damageDir = DamageUtility.CalculateDamageDirection(damageVec);
            m_EnemyController.EnemyAnimController.SetHitParam(damageDir);
            m_EnemyController.EnemyAnimController.PlayTargetAnimation("Hit", true);
        }
    }
}