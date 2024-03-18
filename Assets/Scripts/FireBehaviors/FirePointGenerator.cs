using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class FirePointGenerator : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Transform _firePointsParent;

    public Vector2 cellSize = new Vector2(1f, 1f);
    [SerializeField] private Transform _scalerTransform;
    public int gridSizeX;
    public int gridSizeZ;
    public int gridSizeY;


    public void PlacePoints(GameObject pointObject,ref List<FirePoint> firePoints)
    {
        gridSizeX = (int)(_scalerTransform.lossyScale.x / cellSize.x);
        gridSizeY = (int)(_scalerTransform.lossyScale.y / cellSize.y);


        Vector3 position = new Vector3();

        position.x = (-_scalerTransform.lossyScale.x / 2) + ((_scalerTransform.lossyScale.x % 1) / 2);

        for (int x = 0; x < gridSizeX; x++)
        {
            position.z = (-_scalerTransform.lossyScale.y / 2) + ((_scalerTransform.lossyScale.x % 1) / 2);

            for (int y = 0; y < gridSizeY; y++)
            {
                GameObject go = Instantiate(pointObject, _firePointsParent);
                go.transform.position = position + _firePointsParent.position;
                go.transform.localRotation = Quaternion.identity;

                firePoints.Add(go.GetComponent<FirePoint>());

                position.z += cellSize.y;
            }
            position.x += cellSize.x;
        }
    }
}
