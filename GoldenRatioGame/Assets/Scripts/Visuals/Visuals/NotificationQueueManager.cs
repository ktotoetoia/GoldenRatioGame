using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public sealed class NotificationQueueManager : MonoBehaviour
    {
        private const string ContainerClass = "notificationsContainer";
        private const string WrapperClass   = "notificationWrapper";

        [SerializeField] private UIDocument _document;
        [SerializeField] private string     _containerName = "NotificationsContainer";
        [SerializeField] private float _fadeInTime  = 0.3f;
        [SerializeField] private float _timeToLive  = 1.0f;
        [SerializeField] private float _fadeOutTime = 0.5f;
        [SerializeField] private float _queueTime   = 0.5f;
        private readonly Queue<VisualElement> _pending = new();
        private readonly List<NotificationEntry> _active  = new();
        private VisualElement _container;
        private VisualElement _stagingContainer;

        private float _timeSinceLastDequeue = float.MaxValue;

        private void Awake()
        {
            VisualElement root = _document.rootVisualElement;
            _container = root.Q(_containerName) ?? CreateFallbackContainer(root);
            _stagingContainer = CreateStagingContainer(root);
        }

        private void LateUpdate()
        {
            _timeSinceLastDequeue += Time.deltaTime;

            if (_pending.Count > 0 && _timeSinceLastDequeue >= _queueTime)
            {
                DequeueNext();
                _timeSinceLastDequeue = 0f;
            }

            for (int i = _active.Count - 1; i >= 0; i--)
            {
                if (TickEntry(_active[i], Time.deltaTime))
                {
                    _active[i].Wrapper.RemoveFromHierarchy();
                    _active.RemoveAt(i);
                }
            }
        }

        public void Preview(VisualElement element)
        {
            var wrapper = new VisualElement();
            wrapper.AddToClassList(WrapperClass);
            wrapper.style.opacity = 0f;
            wrapper.Add(element);

            _stagingContainer.Add(wrapper);
            _pending.Enqueue(wrapper);
        }

        private void DequeueNext()
        {
            var wrapper = _pending.Dequeue();

            wrapper.RemoveFromHierarchy();
            _container.Insert(0, wrapper);

            _active.Add(new NotificationEntry(wrapper));
        }

        private bool TickEntry(NotificationEntry entry, float dt)
        {
            entry.Timer += dt;

            switch (entry.State)
            {
                case NotificationState.FadingIn:
                    entry.Wrapper.style.opacity = NormalisedProgress(entry.Timer, _fadeInTime);
                    if (entry.Timer >= _fadeInTime) entry.Transition(NotificationState.Visible);
                    break;
                case NotificationState.Visible:
                    if (entry.Timer >= _timeToLive) entry.Transition(NotificationState.FadingOut);
                    break;
                
                case NotificationState.FadingOut:
                    entry.Wrapper.style.opacity = 1f - NormalisedProgress(entry.Timer, _fadeOutTime);
                    if (entry.Timer >= _fadeOutTime) return true;
                    break;
            }

            return false;
        }

        private static float NormalisedProgress(float timer, float duration) => duration > 0f ? Mathf.Clamp01(timer / duration) : 1f;

        private static VisualElement CreateFallbackContainer(VisualElement root)
        {
            var container = new VisualElement { name = "NotificationsContainer" };
            container.AddToClassList(ContainerClass);
            root.Add(container);
            return container;
        }
        
        private static VisualElement CreateStagingContainer(VisualElement root)
        {
            var staging = new VisualElement { name = "NotificationsStaging" };
            staging.style.position = Position.Absolute;
            staging.style.opacity = 0f;
            staging.pickingMode = PickingMode.Ignore;
            root.Add(staging);
            return staging;
        }

        private enum NotificationState { FadingIn, Visible, FadingOut }

        private sealed class NotificationEntry
        {
            public readonly VisualElement    Wrapper;
            public NotificationState State;
            public float Timer;

            public NotificationEntry(VisualElement wrapper)
            {
                Wrapper = wrapper;
                State   = NotificationState.FadingIn;
                Timer   = 0f;
            }

            public void Transition(NotificationState next)
            {
                State = next;
                Timer = 0f;
            }
        }
    }
}