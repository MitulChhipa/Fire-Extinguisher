using Oculus.Interaction.HandGrab;
using Oculus.Interaction;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FireExtinguisher
{
    public class NoMovementProvider : MonoBehaviour, IMovementProvider
    {
        [SerializeField, Interface(typeof(IPointableElement))]
        private UnityEngine.Object _pointableElement;
        public IPointableElement PointableElement { get; private set; }

        private PoseTravelData _travellingData = PoseTravelData.DEFAULT;

        private bool _started;

       // public List<AutoMoveTowardsTarget> _movers = new List<AutoMoveTowardsTarget>();

        protected virtual void Awake()
        {
            PointableElement = _pointableElement as IPointableElement;
        }

        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            this.AssertField(_pointableElement, nameof(_pointableElement));
            this.EndStart(ref _started);
        }

        public IMovement CreateMovement()
        {
            AutoMoveTowardsTarget mover = new AutoMoveTowardsTarget(_travellingData, PointableElement);

            return mover;
        }

        #region Inject

        public void InjectAllAutoMoveTowardsTargetProvider(IPointableElement pointableElement)
        {
            InjectPointableElement(pointableElement);
        }

        public void InjectPointableElement(IPointableElement pointableElement)
        {
            PointableElement = pointableElement;
            _pointableElement = pointableElement as UnityEngine.Object;
        }
        #endregion
    }

    /// <summary>
    /// This IMovement stores the initial Pose, and in case
    /// of an aborted movement it will finish it itself.
    /// </summary>
    public class AutoMoveTowardsTarget : IMovement
    {
        private PoseTravelData _travellingData;
        private IPointableElement _pointableElement;

        public Pose Pose => _tween.Pose;
        public bool Stopped => _tween == null || _tween.Stopped;
        public bool Aborting { get; private set; }

        public Action<AutoMoveTowardsTarget> WhenAborted = delegate { };

        private UniqueIdentifier _identifier;
        public int Identifier => _identifier.ID;

        private Tween _tween;
        private Pose _target;
        private Pose _source;
        private bool _eventRegistered;

        public AutoMoveTowardsTarget(PoseTravelData travellingData, IPointableElement pointableElement)
        {
            _identifier = UniqueIdentifier.Generate();
            _travellingData = travellingData;
            _pointableElement = pointableElement;
        }

        public void MoveTo(Pose target)
        {
            AbortSelfAligment();
            _target = target;
            _tween = _travellingData.CreateTween(_source, target);
            if (!_eventRegistered)
            {
                _pointableElement.WhenPointerEventRaised += HandlePointerEventRaised;
                _eventRegistered = true;
            }
        }

        public void UpdateTarget(Pose target)
        {
            _target = target;
            _tween.UpdateTarget(_target);
        }

        public void StopAndSetPose(Pose pose)
        {
            if (_eventRegistered)
            {
                _pointableElement.WhenPointerEventRaised -= HandlePointerEventRaised;
                _eventRegistered = false;
            }

            _source = pose;
            if (_tween != null && !_tween.Stopped)
            {
                GeneratePointerEvent(PointerEventType.Hover);
                GeneratePointerEvent(PointerEventType.Select);
                Aborting = true;
                WhenAborted.Invoke(this);
            }
        }

        public void Tick()
        {
            _tween.Tick();
            if (Aborting)
            {
                GeneratePointerEvent(PointerEventType.Move);
                if (_tween.Stopped)
                {
                    AbortSelfAligment();
                }
            }
        }

        private void HandlePointerEventRaised(PointerEvent evt)
        {
            if (evt.Type == PointerEventType.Select || evt.Type == PointerEventType.Unselect)
            {
                AbortSelfAligment();
            }
        }

        private void AbortSelfAligment()
        {
            if (Aborting)
            {
                Aborting = false;

                GeneratePointerEvent(PointerEventType.Unselect);
                GeneratePointerEvent(PointerEventType.Unhover);
            }
        }

        private void GeneratePointerEvent(PointerEventType pointerEventType)
        {
            PointerEvent evt = new PointerEvent(Identifier, pointerEventType, Pose);
            _pointableElement.ProcessPointerEvent(evt);
        }
    }
}