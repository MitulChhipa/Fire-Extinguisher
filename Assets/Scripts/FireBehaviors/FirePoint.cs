using System;
using UnityEngine;

namespace FireExtinguisher.Fire
{
    public class FirePoint : MonoBehaviour
    {
        public bool fireStarted { get; private set; }
        public bool objectBurnt { get; private set; }
        
        public bool fireStopped;

        [SerializeField] private float _timeRequireToFullyBurn = 30f;

        public static Action<FirePoint> OnFirePointBurnt;

        public void SetFire(ref GameObject fire)
        {
            GameObject go = Instantiate(fire, transform.position, Quaternion.identity);
            go.transform.SetParent(transform);
            go.transform.GetComponentInChildren<FireBehavior>().InjectFirePoint(this);
            fireStarted = true;

            Invoke(nameof(CheckBurn), _timeRequireToFullyBurn);
        }

        public void StopFire()
        {
            if (fireStarted)
            {
                gameObject.SetActive(false);
                fireStopped = true;
                CancelInvoke(nameof(CheckBurn));
            }
        }

        public void CheckBurn()
        {
            if(!fireStopped)
            {
                objectBurnt = true;
                OnFirePointBurnt?.Invoke(this);
            }
        }
    }
}