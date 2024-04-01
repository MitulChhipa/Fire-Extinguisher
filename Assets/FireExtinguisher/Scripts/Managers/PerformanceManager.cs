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

        public void SetRefreshRates(float refreshRate)
        {
            Unity.XR.Oculus.Performance.TrySetDisplayRefreshRate(refreshRate);
        }
    }
}
