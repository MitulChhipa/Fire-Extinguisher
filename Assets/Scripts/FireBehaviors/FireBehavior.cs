using UnityEngine;

namespace FireExtinguisher.Fire
{
    public class FireBehavior : MonoBehaviour
    {
        [SerializeField] private float _timeToExtinguish;
        private float _extinguishedTime;
        private float _remainingTime;

        private float _startTime;

        private FirePoint _firePoint;


        private void Start()
        {
            _remainingTime = _timeToExtinguish;
        }


        public void StartExtinguishing()
        {
            if (_firePoint.fireStopped) { return; }

            _startTime = Time.time;
            ExtinguishFireAfter(_remainingTime);
        }

        public void StopExtinguishing()
        {
            if (_firePoint.fireStopped) { return; }
            _extinguishedTime = Time.time - _startTime;
            _remainingTime = _remainingTime - _extinguishedTime;

            CancelInvoke();
        }

        private void ExtinguishFireAfter(float time)
        {
            Invoke("ExtinguishThisFire", time);
        }
        private void ExtinguishThisFire()
        {
            _firePoint.StopFire();
        }

        public void InjectFirePoint(FirePoint firePoint)
        {
            _firePoint = firePoint;
        }
    }
}