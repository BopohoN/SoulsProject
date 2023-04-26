namespace Code.Runtime.Utility
{
    public static class WeaponActionUtility
    {
        public static bool CheckWeaponActionIsLightAttack(int actionId)
        {
            return actionId / 10 % 10 == 1;
        }
    }
}