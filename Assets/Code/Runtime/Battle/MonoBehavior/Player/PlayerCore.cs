using Code.Runtime.Battle.MonoBehavior.Item;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Runtime.Battle.MonoBehavior.Player
{
    public class PlayerCore : MonoBehaviour
    {
        public struct InteractableZoneData
        {
            public RaycastHit hit;
            public int gId;
        }
        public bool isInteracting;
        public bool isSprinting;
        public bool isGrounded;
        public bool canDoCombo;
        public bool isDead;
        public bool isOH = true;

        public bool isInInteractableZone;
        public InteractableZoneData currentInteractableZone; 

        private LayerMask m_IgnoreLayers;
        public BattleCore Core { get; private set; }
        public AnimatorController AnimatorController { get; private set; }
        public PlayerController PlayerController { get; private set; }
        public PlayerInput PlayerInput { get; private set; }
        public PlayerAttacker PlayerAttacker { get; private set; }
        public PlayerInventory PlayerInventory { get; private set; }
        public PlayerStats PlayerStats { get; private set; }
        public Collider PlayerCollider { get; private set; }

        private void Awake()
        {
            PlayerCollider = GetComponent<Collider>();
            AnimatorController = GetComponentInChildren<AnimatorController>();
            PlayerController = GetComponentInChildren<PlayerController>();
            PlayerInput = GetComponentInChildren<PlayerInput>();
            PlayerAttacker = GetComponentInChildren<PlayerAttacker>();
            PlayerInventory = GetComponentInChildren<PlayerInventory>();
            PlayerStats = GetComponentInChildren<PlayerStats>();
            m_IgnoreLayers = ~((1 << 8) | (1 << 9) | (1 << 10));
            currentInteractableZone = new InteractableZoneData
            {
                hit = default,
                gId = -1
            };
        }

        private void Update()
        {
            isInteracting = AnimatorController.IsInteractingFlag;
            canDoCombo = AnimatorController.GetCanDoCombo();

            PlayerController.HandleMovement(Time.deltaTime);
            PlayerController.HandleFalling(Time.deltaTime);
            AnimatorController.SetCanRotate(!isInteracting);
            PlayerInput.TickInput(Time.deltaTime);
            PlayerCollider.enabled = !isDead && isGrounded;
            CheckForInteractableObject();
        }

        public void SetBattleCore(BattleCore core)
        {
            Core = core;
        }

        public void CheckForInteractableObject()
        {
            void CancelInteractable()
            {
                currentInteractableZone.hit.transform.GetComponent<Interactable>().RemoveCancelInteractListener();
                PlayerInput.OnAPressed -= InteractInputListener;
                isInInteractableZone = false;
                currentInteractableZone = default;
            }

            void InteractInputListener(InputAction.CallbackContext ctx)
            {
                var interactable = currentInteractableZone.hit.transform.GetComponent<Interactable>();
                interactable.Interact(this);
            }

            if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out var hit, 1f, m_IgnoreLayers))
            {
                if (string.Equals(hit.collider.tag,"Interactable"))
                {
                    if (currentInteractableZone.gId == -1)
                    {
                        isInInteractableZone = true;
                        currentInteractableZone = new InteractableZoneData
                        {
                            hit = hit,
                            gId = hit.transform.GetInstanceID()
                        };
                        PlayerInput.OnAPressed += InteractInputListener;
                        hit.transform.GetComponent<Interactable>().AddCancelInteractListener(CancelInteractable);
                    }
                }
            }
        }
    }
}