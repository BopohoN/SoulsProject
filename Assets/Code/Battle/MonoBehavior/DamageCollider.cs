using System;
using UnityEngine;

namespace Code.Battle.MonoBehavior
{
    public class DamageCollider : MonoBehaviour
    {
        private Collider m_DamageCollider;
        public int currentWeaponDamage;

        private void Awake()
        {
            m_DamageCollider = GetComponent<Collider>();
            m_DamageCollider.gameObject.SetActive(true);
            m_DamageCollider.isTrigger = true;
            m_DamageCollider.enabled = false;
        }

        public void EnableDamageCollider()
        {
            m_DamageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            m_DamageCollider.enabled = false;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag($"Hittable"))
            {
                if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    var playerStats = other.GetComponent<PlayerStats>();

                    if (playerStats != null)
                        playerStats.TakeDamage(currentWeaponDamage);
                    return;
                }

                if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    var playerStats = other.GetComponent<EnemyStats>();

                    if (playerStats != null)
                        playerStats.TakeDamage(currentWeaponDamage);
                    return;
                }
            }
        }
    }
}