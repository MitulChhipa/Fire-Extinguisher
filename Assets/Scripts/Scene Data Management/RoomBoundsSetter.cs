using UnityEngine;

namespace FireExtinguisher.SceneData
{
    public class RoomBoundsSetter : MonoBehaviour
    {
        [SerializeField] private SphereCollider _collider;
        private float _radius;

        public void SetRoomBounds()
        {
            Transform roomParent = FindObjectOfType<OVRSemanticClassification>().transform.parent;
            Bounds roomBounds = new Bounds(Vector3.zero,Vector3.zero);

            MeshRenderer[] meshRenderers = roomParent.GetComponentsInChildren<MeshRenderer>();

            foreach(MeshRenderer renderer in meshRenderers)
            {
                roomBounds.Encapsulate(renderer.bounds);
            }

            _radius = GetMaxBound(roomBounds.extents) * 2;
            
            _collider.radius = _radius;
        }

        private float GetMaxBound(Vector3 extents)
        {
            float maxRadius;

            if(extents.x > extents.y)
            {
                if(extents.x > extents.z)
                {
                    maxRadius = extents.x;
                }
                else
                {
                    maxRadius = extents.z;
                }
            }
            else
            {
                if( extents.y > extents.z)
                {
                    maxRadius = extents.y;
                }
                else
                {
                    maxRadius = extents.z;
                }
            }


            return maxRadius;
        }
    }
}