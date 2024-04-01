using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace FireExtinguisher.Extinguisher
{
    public class FireExtinguisherRespawnHandler : MonoBehaviour
    {
        [SerializeField] private FireExtinguisherSpawner _fireExtinguisherSpawner;


        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("FireExtinguisher"))
            {
                _fireExtinguisherSpawner.SetFireExtinguisher();
            }
        }
    }
}