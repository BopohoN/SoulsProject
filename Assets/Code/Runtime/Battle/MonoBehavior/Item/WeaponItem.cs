using Code.Runtime.Battle.MonoBehavior.Player;

namespace Code.Runtime.Battle.MonoBehavior.Item
{
    public class WeaponItem : Interactable
    {
        public int weaponId;

        public override void Interact(PlayerCore playerCore)
        {
            base.Interact(playerCore);

            PickUpItem(playerCore);
        }

        private void PickUpItem(PlayerCore playerCore)
        {
            playerCore.PlayerController.StopPlayerMove();
            playerCore.AnimatorController.PlayTargetAnimation("Pick Up", true);
            playerCore.PlayerInventory.AddWeaponToInventory(weaponId);
            Destroy(gameObject);
        }
    }
}