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

        private List<OVRSemanticClassification> _placableClassificationList = new List<OVRSemanticClassification>();
        private List<OVRSemanticClassification> _nonPlacableClassificationList = new List<OVRSemanticClassification>();

        private float _zAxisSize;
        private bool _extinguisherInstantiated = false;
        private Rigidbody _fireExtinguisherRigidbody;

        [SerializeField] private ProjectSettings projectSettings;

        public void SetFireExtinguisher()
        {
            if (!_extinguisherInstantiated)
            {
                _classification = FindObjectsOfType<OVRSemanticClassification>();
                FillOutPlacableAndNonPlacable();

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
            GetBestPlacableToPlace().GetComponent<FireExtinguisherPoint>().PlaceFireExtinguisher(_fireExtinguisher.transform, _zAxisSize);
            _extinguisherInstantiated = true;
        }

        private OVRSemanticClassification GetBestPlacableToPlace()
        {
            float farestDistance = 0;
            OVRSemanticClassification bestFireExtinguisherPoint = _placableClassificationList[0];

            for (int i = 1; i < _placableClassificationList.Count; i++)
            {
                float currentClosestFlamableDistance = GetClosestNonPlacableDistance(_placableClassificationList[i].transform.position);
                if (currentClosestFlamableDistance > farestDistance)
                {
                    farestDistance = currentClosestFlamableDistance;
                    bestFireExtinguisherPoint = _placableClassificationList[i];
                }
            }

            return bestFireExtinguisherPoint;
        }

        private void FillOutPlacableAndNonPlacable()
        {
            string[] placableObjectNames = projectSettings.GetPlacableObjectsObjects();

            for (int i = 0; i < _classification.Length; i++)
            {
                bool isPlacable = false;

                for (int j = 0; j < placableObjectNames.Length; j++)
                {
                    if (_classification[i].Contains(placableObjectNames[j]))
                    {
                        isPlacable = true;
                        break;
                    }
                }

                if (isPlacable)
                {
                    _placableClassificationList.Add(_classification[i]);
                }
                else
                {
                    _nonPlacableClassificationList.Add(_classification[i]);
                }
            }
        }

        public float GetClosestNonPlacableDistance(Vector3 position)
        {
            float closestDistance = Vector3.Distance(position, _nonPlacableClassificationList[0].transform.position);

            foreach (OVRSemanticClassification firePointGenerator in _nonPlacableClassificationList)
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