namespace Code.Utility
{
    public static class MovementUtility
    {
        public static float ClampMovement(float move)
        {
            if (move > 0 && move<0.55f)
            {
                return 0.5f;
            }

            if (move > 0.55f)
            {
                return 1;
            }

            if (move < 0 & move > -0.55f)
            {
                return -0.5f;
            }

            if (move < -0.55f)
            {
                return -1;
            }

            return 0;
        }
    }
}