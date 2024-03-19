using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

namespace FireExtinguisher
{
    public class WallStopper : MonoBehaviour
    {
        [SerializeField] private IInteractionToggle[] interactions;
        [SerializeField] private Rigidbody _rigidbody;

        private bool _cachedState;

        private void Start()
        {
            interactions = GetComponents<IInteractionToggle>();
            print("==== "+interactions.Length);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("SpatialObject"))
            {
                foreach (var interaction in interactions)
                {
                    interaction.DisableInteractionMovement();
                }
                CacheRigidbodyState();
                _rigidbody.isKinematic = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("SpatialObject"))
            {
                foreach (var interaction in interactions)
                {
                    interaction.EnableInteractionMovement();
                }
                _rigidbody.isKinematic = _cachedState;
            }
        }

        private void CacheRigidbodyState()
        {
            _cachedState = _rigidbody.isKinematic;
        }
    }
}