using System.Collections.Generic;
using UnityEngine;


namespace FireExtinguisher.Fire
{
    public class FirePointGenerator : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Transform _firePointsParent;

        public Vector2 cellSize = new Vector2(1f, 1f);
        [SerializeField] private Transform _scalerTransform;
        public int gridSizeX;
        public int gridSizeZ;
        public int gridSizeY;


        public void PlacePoints(GameObject pointObject, ref List<FirePoint> firePoints)
        {
            int count = 0;

            gridSizeX = (int)(_scalerTransform.lossyScale.x / cellSize.x);
            gridSizeY = (int)(_scalerTransform.lossyScale.y / cellSize.y);


            Vector3 position = new Vector3();

            position.x = (-_scalerTransform.lossyScale.x / 2) + cellSize.x / 2;

            for (int x = 0; x < gridSizeX; x++)
            {
                position.z = (-_scalerTransform.lossyScale.y / 2) + +cellSize.y / 2;

                for (int y = 0; y < gridSizeY; y++)
                {
                    GameObject go = Instantiate(pointObject, _firePointsParent);
                    go.transform.position = position + _firePointsParent.position;
                    go.transform.localRotation = Quaternion.identity;

                    firePoints.Add(go.GetComponent<FirePoint>());

                    position.z += cellSize.y;
                    count++;
                }
                position.x += cellSize.x;
            }

            if (count == 0)
            {
                GameObject go = Instantiate(pointObject, _firePointsParent);
                go.transform.position = position;
                go.transform.localRotation = Quaternion.identity;

                firePoints.Add(go.GetComponent<FirePoint>());
            }
        }
    }
}