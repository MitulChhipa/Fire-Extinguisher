using Oculus.Interaction;
using UnityEngine;

namespace FireExtinguisher.Interaction
{
    public class OneGrabTransformer : MonoBehaviour, ITransformer, IInteractionToggle
    {
        [SerializeField]
        private TransformerUtils.PositionConstraints _positionConstraints =
            new TransformerUtils.PositionConstraints()
            {
                XAxis = new TransformerUtils.ConstrainedAxis(),
                YAxis = new TransformerUtils.ConstrainedAxis(),
                ZAxis = new TransformerUtils.ConstrainedAxis()
            };

        [SerializeField]
        private TransformerUtils.RotationConstraints _rotationConstraints =
            new TransformerUtils.RotationConstraints()
            {
                XAxis = new TransformerUtils.ConstrainedAxis(),
                YAxis = new TransformerUtils.ConstrainedAxis(),
                ZAxis = new TransformerUtils.ConstrainedAxis()
            };


        private IGrabbable _grabbable;
        private Pose _grabDeltaInLocalSpace;
        private TransformerUtils.PositionConstraints _parentConstraints;

        private bool _canMove = true;
        private bool _checkDistance;

        public void Initialize(IGrabbable grabbable)
        {
            _grabbable = grabbable;
            Vector3 initialPosition = _grabbable.Transform.localPosition;
            _parentConstraints = TransformerUtils.GenerateParentConstraints(_positionConstraints, initialPosition);
        }

        public void BeginTransform()
        {
            Pose grabPoint = _grabbable.GrabPoints[0];
            var targetTransform = _grabbable.Transform;
            _grabDeltaInLocalSpace = new Pose(targetTransform.InverseTransformVector(grabPoint.position - targetTransform.position),
                                            Quaternion.Inverse(grabPoint.rotation) * targetTransform.rotation);
        }

        public void UpdateTransform()
        {

            Pose grabPoint = _grabbable.GrabPoints[0];
            var targetTransform = _grabbable.Transform;

            // Constrain rotation
            Quaternion updatedRotation = grabPoint.rotation * _grabDeltaInLocalSpace.rotation;

            if (_canMove)
            {
                targetTransform.rotation = TransformerUtils.GetConstrainedTransformRotation(updatedRotation, _rotationConstraints);
            }
            // Constrain position
            Vector3 updatedPosition = grabPoint.position - targetTransform.TransformVector(_grabDeltaInLocalSpace.position);

            if (_canMove)
            {
                targetTransform.position = TransformerUtils.GetConstrainedTransformPosition(updatedPosition, _parentConstraints, targetTransform.parent);
            }
        }

        public void EndTransform() { }

        public void EnableInteractionMovement()
        {
            _canMove = true;
        }

        public void DisableInteractionMovement()
        {
            _canMove = false;
            _checkDistance = true;
        }
    }
}