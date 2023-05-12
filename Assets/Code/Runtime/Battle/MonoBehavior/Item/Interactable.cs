using System;
using Code.Runtime.Battle.Manager;
using Code.Runtime.Battle.MonoBehavior.Player;
using UnityEngine;

namespace Code.Runtime.Battle.MonoBehavior.Item
{
    public class Interactable : MonoBehaviour
    {
        public float radius = 0.6f;
        public string interactableText;
        private Collider m_Collider;
        private Action m_CancelInteract;

        private void Awake()
        {
            m_Collider = GetComponent<Collider>();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        public void AddCancelInteractListener(Action cancelInteract)
        {
            m_CancelInteract = cancelInteract;
        }

        public void RemoveCancelInteractListener()
        {
            m_CancelInteract = null;
        }

        public virtual void Interact(PlayerCore playerCore)
        {
            Debug.Log("Interact!");
            m_CancelInteract?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                m_CancelInteract?.Invoke();
        }
    }
}