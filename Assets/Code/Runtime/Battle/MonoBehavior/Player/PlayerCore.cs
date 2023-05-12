using Code.Runtime.Battle.Manager;
using Code.Runtime.Battle.MonoBehavior.Item;
using Code.Runtime.Battle.Ui;
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
                PlayerInput.OnAPressed -= InteractInputListener;
                var interactable = currentInteractableZone.hit.transform.GetComponent<Interactable>();
                var uiManager = Core.GetMgr<UiManager>();
                var battleMainUi = uiManager.GetUi<MainUi>(EBattleUi.MainUi);
                battleMainUi.SetInteractTipsActive(false);
                
                interactable.RemoveCancelInteractListener();
                isInInteractableZone = false;
                currentInteractableZone = new InteractableZoneData
                {
                    hit = default,
                    gId = -1
                };
            }

            void InteractInputListener(InputAction.CallbackContext ctx)
            {
                PlayerInput.OnAPressed -= InteractInputListener;
                PlayerInput.OnAPressed += ClosePopUp;
                var interactable = currentInteractableZone.hit.transform.GetComponent<Interactable>();
                var uiManager = Core.GetMgr<UiManager>();
                var battleMainUi = uiManager.GetUi<MainUi>(EBattleUi.MainUi);
                interactable.Interact(this);
                
                battleMainUi.SetItemPopup(((WeaponItem) interactable).weaponId);
                battleMainUi.SetItemPopupActive(true);
            }

            void ClosePopUp(InputAction.CallbackContext ctx)
            {
                PlayerInput.OnAPressed -= ClosePopUp;
                var uiManager = Core.GetMgr<UiManager>();
                var battleMainUi = uiManager.GetUi<MainUi>(EBattleUi.MainUi);
                battleMainUi.SetItemPopupActive(false);
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
                        var interactable = currentInteractableZone.hit.transform.GetComponent<Interactable>();
                        var uiManager = Core.GetMgr<UiManager>();
                        var battleMainUi = uiManager.GetUi<MainUi>(EBattleUi.MainUi);
                        battleMainUi.SetInteractTipsActive(true);
                        battleMainUi.SetInteractTipsText(interactable.interactableText);
                        PlayerInput.OnAPressed += InteractInputListener;
                        interactable.AddCancelInteractListener(CancelInteractable);
                    }
                }
            }
        }
    }
}