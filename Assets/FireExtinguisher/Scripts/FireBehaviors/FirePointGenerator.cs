using System.Collections.Generic;
using UnityEngine;


namespace FireExtinguisher.Fire
{
    public class FirePointGenerator : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Transform _firePointsParent;

        public Vector2 cellSize = new Vector2(1f, 1f);
        private int _gridSizeX;
        private int _gridSizeY;

        public void PlacePoints(GameObject pointObject, ref List<FirePoint> firePoints)
        {
            int count = 0;
            Bounds bounds = _renderer.bounds;


            _gridSizeX = (int)(bounds.extents.x * 2 / cellSize.x);
            _gridSizeY = (int)(bounds.extents.z * 2 / cellSize.y);

            Vector3 center = bounds.center;

            Vector3 position = new Vector3();

            position.x = (-bounds.extents.x) + cellSize.x / 2;

            if (_gridSizeX == 0) { _gridSizeX = 1; }
            if (_gridSizeY == 0) { _gridSizeY = 1; }

            for (int x = 0; x < _gridSizeX; x++)
            {
                position.z = (-bounds.extents.z * 2 / 2) + cellSize.y / 2;

                for (int y = 0; y < _gridSizeY; y++)
                {
                    GameObject go = Instantiate(pointObject, _firePointsParent);
                    go.transform.position = position + center;
                    go.transform.localRotation = Quaternion.identity;

                    firePoints.Add(go.GetComponent<FirePoint>());

                    position.z += cellSize.y;
                    count++;
                }
                position.x += cellSize.x;
            }
        }
    }
}