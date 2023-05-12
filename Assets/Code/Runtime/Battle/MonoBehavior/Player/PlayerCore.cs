using UnityEngine;

namespace Code.Runtime.Battle.MonoBehavior.Player
{
    public class PlayerCore : MonoBehaviour
    {
        public bool isInteracting;
        public bool isSprinting;
        public bool isGrounded;
        public bool canDoCombo;
        public bool isDead;
        public bool isOH = true;

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
        }


        public void SetBattleCore(BattleCore core)
        {
            Core = core;
        }
    }
}