using Code.Configuration;

namespace Code.Runtime.Utility
{
    public static class MovementUtility
    {
        public static float ClampMovement(float move)
        {
            if (move > 0 && move < ConstConfig.D[10005].Value / 100f) return 0.5f;

            if (move > ConstConfig.D[10005].Value / 100f) return 1;

            if ((move < 0) & (move > -1 * ConstConfig.D[10005].Value / 100f)) return -0.5f;

            if (move < -1 * ConstConfig.D[10005].Value / 100f) return -1;

            return 0;
        }

        public static float GetFallingVelocity(float fallingTime)
        {
            return ConstConfig.D[10007].Value / 100f * fallingTime;
        }
    }
}