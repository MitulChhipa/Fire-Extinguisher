using FireExtinguisher.Manager;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FireExtinguisher.Utilities;
using Unity.VisualScripting;
using FireExtinguisher.Fire;

namespace FireExtinguisher.Extinguisher
{
    public class FireExtinguisherSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _fireExtinguisherPrefab;
        [SerializeField] private Transform _parent;
        [SerializeField] private FireManager _fireManager;
        private GameObject _fireExtinguisher;
        private OVRSemanticClassification[] _classification;

        private List<OVRSemanticClassification> _placeableClassificationList = new List<OVRSemanticClassification>();
        private List<OVRSemanticClassification> _nonPlaceableClassificationList = new List<OVRSemanticClassification>();

        private float _zAxisSize;
        private bool _extinguisherInstantiated = false;
        private Rigidbody _fireExtinguisherRigidbody;

        [SerializeField] private ProjectSettings projectSettings;

        public void SetFireExtinguisher()
        {
            if (!_extinguisherInstantiated)
            {
                _classification = FindObjectsOfType<OVRSemanticClassification>();
                FillOutPlaceableAndNonPlaceable();

                _fireExtinguisher = Instantiate(_fireExtinguisherPrefab);

                Bounds bounds = new Bounds(_fireExtinguisher.transform.position, Vector3.zero);

                MeshRenderer[] renderers = _fireExtinguisher.transform.GetComponentsInChildren<MeshRenderer>();
                foreach (var renderer in renderers)
                {
                    bounds.Encapsulate(renderer.bounds);
                }

                _zAxisSize = bounds.size.z / 2;

            }

            if (_fireExtinguisherRigidbody == null)
            {
                _fireExtinguisherRigidbody = _fireExtinguisher.GetComponent<Rigidbody>();
            }

            _fireExtinguisherRigidbody.isKinematic = true;
            GetBestPlaceableToPlace().GetComponent<FireExtinguisherPoint>().PlaceFireExtinguisher(_fireExtinguisher.transform, _zAxisSize);
            _extinguisherInstantiated = true;
        }

        private OVRSemanticClassification GetBestPlaceableToPlace()
        {
            float farestDistance = 0;
            OVRSemanticClassification bestFireExtinguisherPoint = _placeableClassificationList[0];

            for (int i = 1; i < _placeableClassificationList.Count; i++)
            {
                float currentClosestFlamableDistance = GetClosestNonPlaceableDistance(_placeableClassificationList[i].transform.position);
                if (currentClosestFlamableDistance > farestDistance)
                {
                    farestDistance = currentClosestFlamableDistance;
                    bestFireExtinguisherPoint = _placeableClassificationList[i];
                }
            }

            return bestFireExtinguisherPoint;
        }

        private void FillOutPlaceableAndNonPlaceable()
        {
            string[] placeableObjectNames = projectSettings.GetPlaceableObjectsObjects();

            for (int i = 0; i < _classification.Length; i++)
            {
                bool isPlaceable = false;

                for (int j = 0; j < placeableObjectNames.Length; j++)
                {
                    if (_classification[i].Contains(placeableObjectNames[j]))
                    {
                        isPlaceable = true;
                        break;
                    }
                }

                if (isPlaceable)
                {
                    _placeableClassificationList.Add(_classification[i]);
                }
                else
                {
                    _nonPlaceableClassificationList.Add(_classification[i]);
                }
            }
        }

        public float GetClosestNonPlaceableDistance(Vector3 position)
        {
            float closestDistance = Vector3.Distance(position, _nonPlaceableClassificationList[0].transform.position);

            foreach (OVRSemanticClassification firePointGenerator in _nonPlaceableClassificationList)
            {
                float currentPointDistance = Vector3.Distance(position, firePointGenerator.transform.position);

                if (currentPointDistance < closestDistance)
                {
                    closestDistance = currentPointDistance;
                }
            }
            return closestDistance;
        }
    }
}