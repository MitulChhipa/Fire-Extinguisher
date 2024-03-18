using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    private OVRSemanticClassification[] _sceneObjects;
    [SerializeField] private List<OVRSemanticClassification> _flamableObjects = new List<OVRSemanticClassification>();
    [SerializeField] private List<FirePointGenerator> _firePointGenerators = new List<FirePointGenerator>();
    private List<FirePoint> _firePoints = new List<FirePoint>();

    [SerializeField] private GameObject _firePrefab;
    [SerializeField] private GameObject _firePoint;

    public int _totalFlamableObjectTypes;

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
                if (_sceneObjects[i].Contains(((FlamableObjects)j).ToString()))
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

        foreach(FirePointGenerator firePointGenerator in _firePointGenerators)
        {
           firePointGenerator.PlacePoints(_firePoint,ref _firePoints);
        }
    }

}

public enum InflamableObjects
{
    WALL_FACE,
    DOOR_FRAME,
    WINDOW_FRAME,
    CEILING
}

public enum FlamableObjects
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