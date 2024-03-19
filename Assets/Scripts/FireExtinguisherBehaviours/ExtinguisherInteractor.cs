using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinguisherInteractor : MonoBehaviour
{
    [SerializeField] private CapsuleCollider _capsuleCollider;
    [SerializeField] private float _height;
    [SerializeField] private float _radius;


    private void OnTriggerEnter(Collider other)
    {
        print("==== " +other.tag);

        if (other.CompareTag("Fire"))
        {
            other.GetComponent<FireBehavior>().StartExtinguishing();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        print("==== " +other.tag);
        if (other.CompareTag("Fire"))
        {
            other.GetComponent<FireBehavior>().StopExtinguishing();
        }
    }

    public void StartInteraction()
    {
        _capsuleCollider.height = _height;
        _capsuleCollider.radius = _radius;
    }
    public void StopInteraction()
    {
        _capsuleCollider.height = 0;
        _capsuleCollider.radius = 0;
    }
}
