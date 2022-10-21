using UnityEngine;

namespace Code.Battle.MonoBehavior
{
    public class PlayerCore : MonoBehaviour
    {
        public bool isInteracting;
        public bool isSprinting;
        public bool isGrounded;
        
        
        public AnimatorController AnimatorController { get; private set; }
        public PlayerController PlayerController { get; private set; }
        public PlayerInput PlayerInput { get; private set; }
        public PlayerAttacker PlayerAttacker { get; private set; }
        public PlayerInventory PlayerInventory { get; private set; }

        private void Awake()
        {
            AnimatorController = GetComponentInChildren<AnimatorController>();
            PlayerController = GetComponentInChildren<PlayerController>();
            PlayerInput = GetComponentInChildren<PlayerInput>();
            PlayerAttacker = GetComponentInChildren<PlayerAttacker>();
            PlayerInventory = GetComponentInChildren<PlayerInventory>();
        }

        void Start()
        {
        
        }

        
        void Update()
        {
            isInteracting = AnimatorController.IsInteractingFlag;
            
            PlayerController.HandleMovement(Time.deltaTime);
            PlayerController.HandleFalling(Time.deltaTime);
            AnimatorController.SetCanRotate(!isInteracting);
            PlayerInput.TickInput(Time.deltaTime);
        }
    }
}
