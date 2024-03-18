using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FireExtinguisher
{
    public class TwoGrabTransformer : MonoBehaviour, ITransformer, IInteractionToggle
    {

        [SerializeField] private UnityEvent OnTargetInteracted;
        [SerializeField] private UnityEvent OnTargetInteractionCompleted;
        [SerializeField] private Transform _target;
        [SerializeField] private float _targetProximity;
        private bool _targeted = false;

        private Quaternion _activeRotation;
        private float _initialDistance;

        private bool _canMove;

        private Pose _previousGrabPointA;
        private Pose _previousGrabPointB;

        [Serializable]
        public class TwoGrabFreeConstraints
        {
            [Tooltip("If true then the constraints are relative to the initial scale of the object " +
                     "if false, constraints are absolute with respect to the object's selected axes.")]
            public bool ConstraintsAreRelative;
            public FloatConstraint MinScale;
            public FloatConstraint MaxScale;
            public bool ConstrainXScale = true;
            public bool ConstrainYScale = false;
            public bool ConstrainZScale = false;
        }

        [SerializeField]
        private TwoGrabFreeConstraints _constraints;

        public TwoGrabFreeConstraints Constraints
        {
            get
            {
                return _constraints;
            }

            set
            {
                _constraints = value;
            }
        }

        private IGrabbable _grabbable;

        public void Initialize(IGrabbable grabbable)
        {
            _grabbable = grabbable;
        }

        public void BeginTransform()
        {
            var grabA = _grabbable.GrabPoints[0];
            var grabB = _grabbable.GrabPoints[1];

            // Initialize our transformer rotation
            Vector3 diff = grabB.position - grabA.position;
            _activeRotation = Quaternion.LookRotation(diff, Vector3.up).normalized;
            _initialDistance = diff.magnitude;

            _previousGrabPointA = new Pose(grabA.position, grabA.rotation);
            _previousGrabPointB = new Pose(grabB.position, grabB.rotation);

            if (Vector3.Distance(grabA.position, _target.position) < _targetProximity || Vector3.Distance(grabB.position, _target.position) < _targetProximity)
            {
                _targeted = true;
                OnTargetInteracted?.Invoke();
            }
        }

        public void UpdateTransform()
        {

            var grabA = _grabbable.GrabPoints[0];
            var grabB = _grabbable.GrabPoints[1];
            var targetTransform = _grabbable.Transform;

            // Use the centroid of our grabs as the transformation center
            Vector3 initialCenter = Vector3.Lerp(_previousGrabPointA.position, _previousGrabPointB.position, 0.5f);
            Vector3 targetCenter = Vector3.Lerp(grabA.position, grabB.position, 0.5f);

            // Our transformer rotation is based off our previously saved rotation
            Quaternion initialRotation = _activeRotation;

            // The base rotation is based on the delta in vector rotation between grab points
            Vector3 initialVector = _previousGrabPointB.position - _previousGrabPointA.position;
            Vector3 targetVector = grabB.position - grabA.position;
            Quaternion baseRotation = Quaternion.FromToRotation(initialVector, targetVector);

            // Any local grab point rotation contributes 50% of its rotation to the final transformation
            // If both grab points rotate the same amount locally, the final result is a 1-1 rotation
            Quaternion deltaA = grabA.rotation * Quaternion.Inverse(_previousGrabPointA.rotation);
            Quaternion halfDeltaA = Quaternion.Slerp(Quaternion.identity, deltaA, 0.5f);

            Quaternion deltaB = grabB.rotation * Quaternion.Inverse(_previousGrabPointB.rotation);
            Quaternion halfDeltaB = Quaternion.Slerp(Quaternion.identity, deltaB, 0.5f);

            // Apply all the rotation deltas
            Quaternion baseTargetRotation = baseRotation * halfDeltaA * halfDeltaB * initialRotation;

            // Normalize the rotation
            Vector3 upDirection = baseTargetRotation * Vector3.up;
            Quaternion targetRotation = Quaternion.LookRotation(targetVector, upDirection).normalized;

            // Save this target rotation as our active rotation state for future updates
            _activeRotation = targetRotation;

            // Scale logic
            float activeDistance = targetVector.magnitude;
            if (Mathf.Abs(activeDistance) < 0.0001f) activeDistance = 0.0001f;

            float scalePercentage = activeDistance / _initialDistance;

            


            // Apply the positional delta initialCenter -> targetCenter and the
            // rotational delta initialRotation -> targetRotation to the target transform
            Vector3 worldOffsetFromCenter = targetTransform.position - initialCenter;

            Vector3 offsetInTargetSpace = Quaternion.Inverse(initialRotation) * worldOffsetFromCenter;

            Quaternion rotationInTargetSpace = Quaternion.Inverse(initialRotation) * targetTransform.rotation;

            if (_canMove)
            {
                targetTransform.position = (targetRotation * (offsetInTargetSpace)) + targetCenter;
                targetTransform.rotation = targetRotation * rotationInTargetSpace;
            }
            _previousGrabPointA = new Pose(grabA.position, grabA.rotation);
            _previousGrabPointB = new Pose(grabB.position, grabB.rotation);
        }

        public void EndTransform()
        {
            if (_targeted)
            {
                _targeted = false;
                OnTargetInteractionCompleted?.Invoke();
            }
        }
        public void EnableInteractionMovement()
        {
            _canMove = true;
        }

        public void DisableInteractionMovement()
        {
            _canMove = false;
        }


        #region Inject

        public void InjectOptionalConstraints(TwoGrabFreeConstraints constraints)
        {
            _constraints = constraints;
        }

        #endregion
    }
}