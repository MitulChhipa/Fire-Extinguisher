using System;
using System.Threading.Tasks;
using UnityEngine;


namespace FireExtinguisher.Manager
{
    public class PerformanceManager : MonoBehaviour
    {
        private float[] _availableRefreshRates;
        private float _currentRefreshRate;

        private static Action OnRefreshRateChanged;
        private static Action OnRefreshRateChangeFailed;

        private void Awake()
        {
            Unity.XR.Oculus.Performance.TryGetAvailableDisplayRefreshRates(out _availableRefreshRates);
            Unity.XR.Oculus.Performance.TryGetDisplayRefreshRate(out _currentRefreshRate);

        }

        public void SetRefreshRates(int id)
        {
            bool set = Unity.XR.Oculus.Performance.TrySetDisplayRefreshRate(_availableRefreshRates[id]);
            Unity.XR.Oculus.Performance.TryGetDisplayRefreshRate(out _currentRefreshRate);

            if (set)
            {
                OnRefreshRateChanged?.Invoke();
            }
            else
            {
                OnRefreshRateChangeFailed?.Invoke();
            }
        }
    }
}