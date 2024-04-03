using System;
using System.Threading.Tasks;
using UnityEngine;


namespace FireExtinguisher.Manager
{
    public class PerformanceManager : MonoBehaviour
    {
        private float[] _availableRefreshRates;
        private float _currentRefreshRates;

        private void Awake()
        {
            Unity.XR.Oculus.Performance.TryGetAvailableDisplayRefreshRates(out _availableRefreshRates);
        }



        public async void SetRefreshRates(float refreshRate,Action<float> OnComplete)
        {
            Unity.XR.Oculus.Performance.TrySetDisplayRefreshRate(refreshRate);

            await Task.Delay(2000);

            float currentRefreshRate;
            Unity.XR.Oculus.Performance.TryGetDisplayRefreshRate(out currentRefreshRate);
            OnComplete?.Invoke(currentRefreshRate);
        }
    }
}