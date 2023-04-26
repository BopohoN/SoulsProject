using Code.Runtime.Utility;
using UnityEngine;

namespace Code.Runtime.Battle.MonoBehavior
{
    public class EnemyStats : MonoBehaviour
    {
        private static readonly int DamageX = Animator.StringToHash("DamageX");
        private static readonly int DamageY = Animator.StringToHash("DamageY");

        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int Death = Animator.StringToHash("Death");
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

        public void TakeDamage(int damage, Vector2 damageVec)
        {
            Debug.Log("Player take damage: " + damage);
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                m_Animator.Play(Death);
                return;
            }

            var damageDir = DamageUtility.CalculateDamageDirection(damageVec);

            m_Animator.SetFloat(DamageX, damageDir.x);
            m_Animator.SetFloat(DamageY, damageDir.y);
            m_Animator.Play(Hit);
        }
    }
}