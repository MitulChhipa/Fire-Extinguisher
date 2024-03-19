using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePoint : MonoBehaviour
{
    public bool fireStarted;

    public void SetFire(ref GameObject fire)
    {
        GameObject go = Instantiate(fire, transform.position,Quaternion.identity);
        go.transform.SetParent(transform);

        fireStarted = true;
    }
}
