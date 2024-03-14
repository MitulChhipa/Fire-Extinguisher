using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireExtinguisher
{
    public class Extinguisher : MonoBehaviour
    {
        private GrabInteractable _grabInteractable;
        private DistanceGrabInteractable _distanceGrabInteractable;
        private HandGrabInteractable _handGrabInteractable;
        private DistanceHandGrabInteractable _distanceHandGrabInteractable;

        [SerializeField] private Transform _interactableTransform;

        public bool isExtinguishing;

        private void Start()
        {
            _grabInteractable = _interactableTransform.GetComponent<GrabInteractable>();
            _distanceGrabInteractable = _interactableTransform.GetComponent<DistanceGrabInteractable>();
            _handGrabInteractable = _interactableTransform.GetComponent<HandGrabInteractable>();
            _distanceHandGrabInteractable = _interactableTransform.GetComponent<DistanceHandGrabInteractable>();


            _grabInteractable.WhenPointerEventRaised += GrabEvent;
            _distanceGrabInteractable.WhenPointerEventRaised += GrabEvent;
            _handGrabInteractable.WhenPointerEventRaised += GrabEvent;
            _distanceHandGrabInteractable.WhenPointerEventRaised += GrabEvent;
        }


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

        private void StartExtinguisher()
        {
            isExtinguishing = true;
        }
        private void StopExtinguisher()
        {
            isExtinguishing = false;
        }
    }
}