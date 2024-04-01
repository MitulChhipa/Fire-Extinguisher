using UnityEngine;


namespace FireExtinguisher.Manager
{
    public class PerformanceManager : MonoBehaviour
    {
        private float[] _availableRefreshRates;

        private void Awake()
        {
            Unity.XR.Oculus.Performance.TryGetAvailableDisplayRefreshRates(out _availableRefreshRates);
        }
    }
}
