using UnityEngine;

namespace FireExtinguisher.Utilities
{
    public static class Extensions
    {
        public static float GetMax(this Vector3 vector)
        {
            float max;

            if (vector.x > vector.y)
            {
                if (vector.x > vector.z)
                {
                    max = vector.x;
                }
                else
                {
                    max = vector.z;
                }
            }
            else
            {
                if (vector.y > vector.z)
                {
                    max = vector.y;
                }
                else
                {
                    max = vector.z;
                }
            }

            return max;
        }
    }
}