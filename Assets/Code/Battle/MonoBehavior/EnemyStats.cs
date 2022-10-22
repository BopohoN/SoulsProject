using UnityEngine;

namespace Code.Battle.MonoBehavior
{
    public class EnemyStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        private Animator m_Animator;

        private void Awake()
        {
            m_Animator = GetComponentInChildren<Animator>();
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

        public void TakeDamage(int damage)
        {
            Debug.Log("Player take damage: " + damage);
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                m_Animator.Play("Death");
                return;
            }
            m_Animator.Play("core_oh_hit_reaction_medium_F_01");
        }
    }
}