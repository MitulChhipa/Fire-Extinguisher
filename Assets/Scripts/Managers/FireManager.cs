using System.Collections.Generic;
using UnityEngine;
using FireExtinguisher.Fire;


namespace FireExtinguisher.Manager
{
    public class FireManager : MonoBehaviour
    {
        private OVRSemanticClassification[] _sceneObjects;
        [SerializeField] private List<OVRSemanticClassification> _flamableObjects = new List<OVRSemanticClassification>();
        [SerializeField] private List<FirePointGenerator> _firePointGenerators = new List<FirePointGenerator>();
        private List<FirePoint> _firePoints = new List<FirePoint>();

        private List<FirePoint> _litPoints = new List<FirePoint>();

        [SerializeField] private GameObject _firePrefab;
        [SerializeField] private GameObject _firePoint;

        public int _totalFlamableObjectTypes;

        public int _count;
        private Vector3 _fireOrigin;


        public void CacheSceneObjects()
        {
            _sceneObjects = FindObjectsOfType<OVRSemanticClassification>();
            FetchFlamableObjects();
            Invoke("SetFlamePoints", 1);
        }

        private void FetchFlamableObjects()
        {
            for (int i = 0; i < _sceneObjects.Length; i++)
            {
                bool isFlamable = false;
                for (int j = 0; j < _totalFlamableObjectTypes; j++)
                {
                    if (_sceneObjects[i].Contains(((FlammableObjects)j).ToString()))
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
                firePointGenerator.PlacePoints(_firePoint, ref _firePoints);
            }

            InvokeRepeating("SetFireToPoints", 1, 1);
        }

        private void SetFireToPoints()
        {
            if (_firePoints.Count == 0) { return; }

            if (_litPoints.Count == 0)
            {
                int index = Random.Range(0, _firePoints.Count);
                _firePoints[index].SetFire(ref _firePrefab);
                _fireOrigin = _firePoints[index].transform.position;

                _litPoints.Add(_firePoints[index]);
                _firePoints.Remove(_firePoints[index]);
            }
            else
            {
                //FirePoint currentFirePointToLit = GetClosestFirePoint(_litPoints[Random.Range(0, _litPoints.Count)].transform.position);
                FirePoint currentFirePointToLit = GetClosestFirePoint(_fireOrigin);
                currentFirePointToLit.SetFire(ref _firePrefab);
                _litPoints.Add(currentFirePointToLit);
                _firePoints.Remove(currentFirePointToLit);
            }
        }

        private FirePoint GetClosestFirePoint(Vector3 position)
        {
            FirePoint closestFirePoint = _firePoints[0];
            float lastPointDistance = Vector3.Distance(position, _firePoints[0].transform.position);

            foreach (var firePoint in _firePoints)
            {
                float currentPointDistance = Vector3.Distance(position, firePoint.transform.position);

                if (currentPointDistance < lastPointDistance)
                {
                    closestFirePoint = firePoint;
                    lastPointDistance = currentPointDistance;
                }
            }

            return closestFirePoint;
        }
    }

    public enum InflammableObjects
    {
        WALL_FACE,
        DOOR_FRAME,
        WINDOW_FRAME,
        CEILING,
        FLOOR
    }

    public enum FlammableObjects
    {
        DESK,
        COUCH,
        OTHER,
        STORAGE,
        BED,
        SCREEN,
        LAMP,
        PLANT,
        TABLE,
        WALL_ART
    }
}