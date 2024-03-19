using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireExtinguisher
{
    public class Extinguisher : MonoBehaviour
    {
        public bool isExtinguishing;

        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private float _emissionTime;
        [SerializeField] private ExtinguisherInteractor _extinguisherInteractor;

        public void StartExtinguisher()
        {
            _extinguisherInteractor.StartInteraction();
            isExtinguishing = true;
            InvokeRepeating("EmitParticles", _emissionTime, _emissionTime);
        }
        public void StopExtinguisher()
        {
            _extinguisherInteractor.StopInteraction();
            isExtinguishing = false;
            CancelInvoke("EmitParticles");
        }

        private void EmitParticles()
        {
            _particle.Emit(1);
        }
    }
}