using FireExtinguisher.Fire;
using System.Collections.Generic;
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
        public static FirePoint GetClosestFirePoint(this List<FirePoint> firePoints ,Vector3 position)
        {
            FirePoint closestFirePoint = firePoints[0];
            float lastPointDistance = Vector3.Distance(position, firePoints[0].transform.position);

            foreach (var firePoint in firePoints)
            {
                float currentPointDistance = Vector3.Distance(position, firePoint.transform.position);

                if (currentPointDistance < lastPointDistance)
                {
                    closestFirePoint = firePoint;
                    lastPointDistance = currentPointDistance;
                }
            }

            return closestFirePoint;
        }
    }
}