using FireExtinguisher.Utilities;
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

            _radius = roomBounds.extents.GetMax() * 2;
            
            _collider.radius = _radius;
        }
    }
}