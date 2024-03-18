using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePoint : MonoBehaviour
{
    public bool fireStarted;

    public void SetFire(GameObject fire)
    {
        Instantiate(fire, transform.position,Quaternion.identity,transform);
        fireStarted = true;
    }
}
