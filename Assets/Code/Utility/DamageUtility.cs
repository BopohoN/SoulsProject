using UnityEngine;

namespace Code.Utility
{
    public static class DamageUtility
    {
        public static Vector2 CalculateDamageDirection(Vector2 damageVec)
        {
            if (Mathf.Abs(damageVec.x) > Mathf.Abs(damageVec.y))
                return damageVec.x > 0 ? Vector2.right : Vector2.left;

            if (Mathf.Abs(damageVec.x) < Mathf.Abs(damageVec.y))
                return damageVec.y > 0 ? Vector2.up : Vector2.down;
            
            return Vector2.down;
        }
    }
}