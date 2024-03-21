using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisherPoint : MonoBehaviour
{

    public void PlaceFireExtinguisher(Transform fireExtinguisher,float offset)
    {
        fireExtinguisher.rotation = transform.rotation;
        fireExtinguisher.position = transform.position + transform.forward* offset;
        fireExtinguisher.SetParent(transform);
    }
}
