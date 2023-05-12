using Code.Runtime.Battle.MonoBehavior.Player;
using UnityEngine;

namespace Code.Runtime.Battle.MonoBehavior
{
    public class DamagePlayerEnt : MonoBehaviour
    {
        public int damage = 25;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                var playerStats = other.GetComponent<PlayerStats>();
                playerStats.TakeDamage(damage, Vector2.down);
            }
        }
    }
}