using UnityEngine;

namespace FireExtinguisher.Fire
{
    public class FirePoint : MonoBehaviour
    {
        public bool fireStarted;
        public bool fireStopped;

        public void SetFire(ref GameObject fire)
        {
            GameObject go = Instantiate(fire, transform.position, Quaternion.identity);
            go.transform.SetParent(transform);
            go.transform.GetComponentInChildren<FireBehavior>().InjectFirePoint(this);
            fireStarted = true;
        }

        public void StopFire()
        {
            if (fireStarted)
            {
                gameObject.SetActive(false);
                fireStopped = true;
            }
        }
    }
}