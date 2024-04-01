using FireExtinguisher.Manager;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FireExtinguisher.Utilities;



namespace FireExtinguisher.Extinguisher
{
    public class FireExtinguisherSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _fireExtinguisherPrefab;
        [SerializeField] private Transform _parent;
        [SerializeField] private FireManager _fireManager;
        private GameObject _fireExtinguisher;
        private OVRSemanticClassification[] _classification;
        private List<FireExtinguisherPoint> _fireExtinguisherPoints = new List<FireExtinguisherPoint>();
        private float _zAxisSize;
        private bool _extinguisherInstantiated = false;
        private Rigidbody _fireExtinguisherRigidbody;

        public void SetFireExtinguisher()
        {
            if (!_extinguisherInstantiated)
            {
                _classification = FindObjectsOfType<OVRSemanticClassification>().Where(c => c.Contains(SceneObjects.WALL_FACE.ToString())).ToArray();

                if (_fireExtinguisherPoints.Count == 0)
                {
                    foreach (var classification in _classification)
                    {
                        FireExtinguisherPoint point = classification.GetComponent<FireExtinguisherPoint>();


                        _fireExtinguisherPoints.Add(point);
                    }
                }


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
            GetBestWallToPlace().PlaceFireExtinguisher(_fireExtinguisher.transform, _zAxisSize);
            _extinguisherInstantiated = true;
        }

        private FireExtinguisherPoint GetClosestWallToPlace(Vector3 position)
        {
            float closestDistance = Vector3.Distance(_fireExtinguisherPoints[0].transform.position, position);
            FireExtinguisherPoint closestFireExtinguisherPoint = _fireExtinguisherPoints[0];

            for (int i = 1; i < _fireExtinguisherPoints.Count; i++)
            {
                float currentDistance = Vector3.Distance(_fireExtinguisherPoints[i].transform.position, position);

                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    closestFireExtinguisherPoint = _fireExtinguisherPoints[i];
                }
            }

            return closestFireExtinguisherPoint;

        }
        private FireExtinguisherPoint GetBestWallToPlace()
        {
            float farestDistance = 0;
            FireExtinguisherPoint bestFireExtinguisherPoint = _fireExtinguisherPoints[0];

            for (int i = 1; i < _fireExtinguisherPoints.Count; i++)
            {
                float currentClosestFlamableDistance = _fireManager.GetClosestFlamableDistance(_fireExtinguisherPoints[i].transform.position);
                if (currentClosestFlamableDistance > farestDistance)
                {
                    farestDistance = currentClosestFlamableDistance;
                    bestFireExtinguisherPoint = _fireExtinguisherPoints[i];
                }
            }

            return bestFireExtinguisherPoint;
        }
    }
}