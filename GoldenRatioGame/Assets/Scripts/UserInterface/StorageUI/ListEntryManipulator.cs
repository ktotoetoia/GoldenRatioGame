using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{
    public class ListEntryManipulator : PointerManipulator
    {
        private enum State
        {
            Idle,
            Pressed,
            Dragging
        }

        private readonly Action<VisualElement> _onDoubleClick;
        private readonly Action<VisualElement> _onSelected;
        private readonly Action<VisualElement> _onMove;
        private readonly Action<VisualElement> _onReleased;

        private State _state = State.Idle;
        private float _lastClickTime;
        private int _pointerId;
        private Vector2 _pressPosition;

        public float DoubleClickTimeDelta { get; set; } = 0.3f;
        public float SelectionRange     { get; set; } = 0.2f;

        public ListEntryManipulator(
            Action<VisualElement> onDoubleClick,
            Action<VisualElement> onSelected,
            Action<VisualElement> onMove,
            Action<VisualElement> onReleased)
        {
            _onDoubleClick = onDoubleClick;
            _onSelected    = onSelected;
            _onMove        = onMove;
            _onReleased    = onReleased;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(OnDown);
            target.RegisterCallback<PointerMoveEvent>(OnMoveEvent);
            target.RegisterCallback<PointerUpEvent>(OnUpOrCancel);
            target.RegisterCallback<PointerCancelEvent>(OnUpOrCancel);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(OnDown);
            target.UnregisterCallback<PointerMoveEvent>(OnMoveEvent);
            target.UnregisterCallback<PointerUpEvent>(OnUpOrCancel);
            target.UnregisterCallback<PointerCancelEvent>(OnUpOrCancel);
        }

        private void OnDown(PointerDownEvent evt)
        {
            if (evt.button != 0)
                return;

            if (TryDoubleClick())
                return;

            _state = State.Pressed;
            _pointerId = evt.pointerId;
            _pressPosition = evt.position;

            target.CapturePointer(_pointerId);
        }

        private void OnMoveEvent(PointerMoveEvent evt)
        {
            if (_state == State.Idle)
                return;

            if (_state == State.Pressed &&
                Vector2.Distance(evt.position, _pressPosition) >= SelectionRange)
            {
                _state = State.Dragging;
                _onSelected?.Invoke(target);
            }

            if (_state == State.Dragging)
            {
                _onMove?.Invoke(target);
            }
        }

        private void OnUpOrCancel(EventBase _)
        {
            if (_state == State.Idle)
                return;

            if (target.HasPointerCapture(_pointerId))
                target.ReleasePointer(_pointerId);

            if (_state == State.Dragging)
                _onReleased?.Invoke(target);

            Reset();
        }

        private bool TryDoubleClick()
        {
            float now = Time.unscaledTime;

            if (now - _lastClickTime < DoubleClickTimeDelta)
            {
                _onDoubleClick?.Invoke(target);
                _lastClickTime = 0;
                return true;
            }

            _lastClickTime = now;
            return false;
        }

        private void Reset()
        {
            _state = State.Idle;
        }
    }
}