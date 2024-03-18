using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePoint : MonoBehaviour
{
    public bool fireStarted;

    public void StartFire(GameObject fire)
    {
        Instantiate(fire, transform);
        fireStarted = true;
    }
}
