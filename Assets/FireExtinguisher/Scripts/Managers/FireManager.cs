using System.Collections.Generic;
using UnityEngine;
using FireExtinguisher.Fire;
using FireExtinguisher.Extinguisher;
using FireExtinguisher.Utilities;
using Unity.VisualScripting;


namespace FireExtinguisher.Manager
{
    public class FireManager : MonoBehaviour
    {
        private OVRSemanticClassification[] _sceneObjects;
        [SerializeField] private List<OVRSemanticClassification> _flamableObjects = new List<OVRSemanticClassification>();
        [SerializeField] private List<FirePointGenerator> _firePointGenerators = new List<FirePointGenerator>();

        private List<FirePoint> _unlitFirePoints = new List<FirePoint>();
        private List<FirePoint> _litFirePoints = new List<FirePoint>();
        private List<FirePoint> _burntFirePoints = new List<FirePoint>();
        private List<FirePoint> _extinguishedFirePoints = new List<FirePoint>();

        [SerializeField] private FireExtinguisherSpawner _fireExtinguisherSpawner;
        [SerializeField] private GameObject _firePrefab;
        [SerializeField] private GameObject _firePoint;

        private Vector3 _fireOrigin;
        [SerializeField] private ProjectSettings _projectSettings;

        [Range(0f, 1f)]
        [SerializeField] private float _warningThreshold;
        [Range(0f, 1f)]
        [SerializeField] private float _losingThreshold;
        private int _totalFirePoints;

        private void OnEnable()
        {
            FirePoint.OnFirePointBurnt += FirePointBurnt;
            FirePoint.OnFireStopped += FirePointStoppedBurning;
        }
        private void OnDisable()
        {
            FirePoint.OnFirePointBurnt -= FirePointBurnt;
            FirePoint.OnFireStopped -= FirePointStoppedBurning;
        }

        public void CacheSceneObjects()
        {
            _sceneObjects = FindObjectsOfType<OVRSemanticClassification>();
            FetchFlamableObjects();
            Invoke("SetFlamePoints", 1);
        }

        private void FetchFlamableObjects()
        {
            string[] flamableObj = _projectSettings.GetFlamableObjects();

            int sceneObjectsLength = _sceneObjects.Length;
            int flamableObjectsLength = flamableObj.Length;

            for (int i = 0; i < sceneObjectsLength; i++)
            {
                bool isFlamable = false;
                for (int j = 0; j < flamableObjectsLength; j++)
                {
                    if (_sceneObjects[i].Contains(flamableObj[j]))
                    {
                        isFlamable = true;
                        break;
                    }
                }

                if (isFlamable)
                {
                    _flamableObjects.Add(_sceneObjects[i]);
                }
            }
        }
        private void SetFlamePoints()
        {
            foreach (var flamableObject in _flamableObjects)
            {
                var firePointGenerator = flamableObject.GetComponent<FirePointGenerator>();
                _firePointGenerators.Add(firePointGenerator);
            }

            foreach (FirePointGenerator firePointGenerator in _firePointGenerators)
            {
                firePointGenerator.PlacePoints(_firePoint, ref _unlitFirePoints);
            }


            _totalFirePoints = _unlitFirePoints.Count;
            _fireExtinguisherSpawner.SetFireExtinguisher();

            InvokeRepeating("SetFireToPoints", 1, 1);
        }

        private void SetFireToPoints()
        {
            CheckWarningCondition();
            
            if (_unlitFirePoints.Count == 0) { return; }

            if (_litFirePoints.Count == 0)
            {
                int index = Random.Range(0, _unlitFirePoints.Count);
                _unlitFirePoints[index].SetFire(ref _firePrefab);
                _fireOrigin = _unlitFirePoints[index].transform.position;

                _litFirePoints.Add(_unlitFirePoints[index]);
                _unlitFirePoints.Remove(_unlitFirePoints[index]);
            }
            else
            {
                FirePoint currentFirePointToLit = _unlitFirePoints.GetClosestFirePoint(_fireOrigin);
                currentFirePointToLit.SetFire(ref _firePrefab);
                _litFirePoints.Add(currentFirePointToLit);
                _unlitFirePoints.Remove(currentFirePointToLit);
            }
        }
        public float GetClosestFlamableDistance(Vector3 position)
        {
            float closestDistance = Vector3.Distance(position, _firePointGenerators[0].transform.position);

            foreach (FirePointGenerator firePointGenerator in _firePointGenerators)
            {
                float currentPointDistance = Vector3.Distance(position, firePointGenerator.transform.position);

                if (currentPointDistance < closestDistance)
                {
                    closestDistance = currentPointDistance;
                }
            }
            return closestDistance;
        }
        private void FirePointBurnt(FirePoint firePoint)
        {
            _burntFirePoints.Add(firePoint);
            CheckLoseCondition();
        }
        private void FirePointStoppedBurning(FirePoint firePoint)
        {
            _extinguishedFirePoints.Add(firePoint);
            CheckWinCondition();
        }

        #region Conditions
        private void CheckLoseCondition()
        {
            if (_burntFirePoints.Count > (_totalFirePoints * _losingThreshold))
            {
                print("Lose");
                GameManager.OnLost?.Invoke();
            }
        }

        private void CheckWinCondition()
        {
            if (_extinguishedFirePoints.Count == _litFirePoints.Count)
            {
                print("Won");
                GameManager.OnWon?.Invoke();
            }
        }   
       
        private void CheckWarningCondition()
        {
            if ((_extinguishedFirePoints.Count < _litFirePoints.Count * _warningThreshold) && (_litFirePoints.Count > (_totalFirePoints * _warningThreshold)))
            {
                print("warning");
                GameManager.OnWarning?.Invoke();
            }
        }
        #endregion
    }
}