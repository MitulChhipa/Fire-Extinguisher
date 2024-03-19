using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExtinguisherTriggerSetter : MonoBehaviour
{
    [SerializeField] private float _height;
    [SerializeField] private float _radius;

    private void Start()
    {
        Invoke("AddTriggerForExtinguisher", 1);
    }

    public void AddTriggerForExtinguisher()
    {
        CapsuleCollider collider = transform.AddComponent<CapsuleCollider>();
        collider.height = _height;
        collider.radius = _radius;
        collider.isTrigger = true;
    }
}
