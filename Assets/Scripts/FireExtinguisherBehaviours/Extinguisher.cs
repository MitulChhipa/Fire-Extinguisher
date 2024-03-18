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

        public void StartExtinguisher()
        {
            isExtinguishing = true;
            InvokeRepeating("EmitParticles", _emissionTime, _emissionTime);
        }
        public void StopExtinguisher()
        {
            isExtinguishing = false;
            CancelInvoke("EmitParticles");
        }

        private void EmitParticles()
        {
            _particle.Emit(1);
        }
    }
}