using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireExtinguisher
{
    public class Extinguisher : MonoBehaviour
    {
        private GrabInteractable _grabInteractable;
        private HandGrabInteractable _handGrabInteractable;

        [SerializeField] private Transform _interactableTransform;

        public bool isExtinguishing;

        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private float _emissionTime;

        //private void Start()
        //{
        //    _grabInteractable = _interactableTransform.GetComponent<GrabInteractable>();
        //    _handGrabInteractable = _interactableTransform.GetComponent<HandGrabInteractable>();


        //    _grabInteractable.WhenPointerEventRaised += GrabEvent;
        //    _handGrabInteractable.WhenPointerEventRaised += GrabEvent;
        //}

        private void GrabEvent(PointerEvent pointerEvent)
        {
            switch (pointerEvent.Type)
            {
                case PointerEventType.Select:
                    {
                        StartExtinguisher();
                        break;
                    }
                case PointerEventType.Unselect:
                    {
                        StopExtinguisher();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

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