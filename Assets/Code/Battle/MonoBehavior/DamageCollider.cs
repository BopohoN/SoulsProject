using System;
using UnityEngine;

namespace Code.Battle.MonoBehavior
{
    public class DamageCollider : MonoBehaviour
    {
        private Collider m_DamageCollider;
        public int currentWeaponDamage;
        private Vector3 m_LastFramePos;

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

        public void FixedUpdate()
        {
            m_LastFramePos = transform.position;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag($"Hittable"))
            {
                var lastDamagePos = other.transform.InverseTransformPoint(m_LastFramePos);
                var currentDamagePos = other.transform.InverseTransformPoint(transform.position);
                var damageVec = new Vector2(currentDamagePos.x - lastDamagePos.x, currentDamagePos.z - lastDamagePos.z);
                
                if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    var playerStats = other.GetComponent<PlayerStats>();

                    if (playerStats != null)
                        playerStats.TakeDamage(currentWeaponDamage, damageVec);
                    return;
                }

                if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    var enemyStats = other.GetComponent<EnemyStats>();

                    if (enemyStats != null)
                        enemyStats.TakeDamage(currentWeaponDamage, damageVec);
                    return;
                }
            }
        }
    }
}