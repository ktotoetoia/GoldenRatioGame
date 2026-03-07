using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace IM.Visuals
{
    [UxmlElement]
    public partial class MarqueeContainer : VisualElement
    {
        private const int DefaultDurationMs = 3000;
        private const float DefaultWaitBefore = 1f;
        private const float DefaultWaitAfter = 1f;

        [UxmlAttribute] public bool Loop { get; set; } = true;

        [UxmlAttribute]
        public float WaitBefore
        {
            get => _waitBefore;
            set { _waitBefore = value; RequestReset(); }
        }

        [UxmlAttribute]
        public int DurationMs
        {
            get => _durationMs;
            set { _durationMs = value; RequestReset(); }
        }

        [UxmlAttribute]
        public float WaitAfter
        {
            get => _waitAfter;
            set { _waitAfter = value; RequestReset(); }
        }

        [UxmlAttribute]
        public string Text
        {
            get => _label.text;
            set { _label.text = value; RequestReset(); }
        }

        private readonly Label _label;
        private ValueAnimation<float> _currentAnim;
        private IVisualElementScheduledItem _initialWaitTask;
        private IVisualElementScheduledItem _postWaitTask;

        private string _lastText = string.Empty;
        private Vector2 _lastSize = Vector2.zero;

        private float _waitBefore = DefaultWaitBefore;
        private float _waitAfter = DefaultWaitAfter;
        private int _durationMs = DefaultDurationMs;

        public MarqueeContainer()
        {
            style.overflow = Overflow.Hidden;
            style.flexGrow = new StyleFloat(1);
            
            _label = new Label
            {
                style =
                {
                    position = Position.Absolute,
                    whiteSpace = WhiteSpace.NoWrap,
                    flexShrink = 0
                }
            };
            AddToClassList("marquee-container");
            _label.AddToClassList("marquee-container-label");
            Add(_label);

            RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            RegisterCallback<DetachFromPanelEvent>(_ => StopEverything());
        }

        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            if (evt.newRect.size == _lastSize && _label.text == _lastText)
                return;

            _lastSize = evt.newRect.size;
            _lastText = _label.text;
            RequestReset();
        }

        private void RequestReset()
        {
            StopEverything();
            schedule.Execute(StartCycle);
        }

        private void StopEverything()
        {
            StopAnimation();
            CancelScheduled(ref _initialWaitTask);
            CancelScheduled(ref _postWaitTask);
            _label.style.left = 0;
        }

        private void StopAnimation()
        {
            if (_currentAnim == null) return;
            _currentAnim.Stop();
            _currentAnim = null;
        }

        private void CancelScheduled(ref IVisualElementScheduledItem task)
        {
            if (task == null) return;
            task.Pause();
            task = null;
        }

        private void StartCycle()
        {
            StopEverything();
            _label.style.left = 0;

            float textWidth = _label.MeasureTextSize(_label.text ?? string.Empty,
                                                     float.PositiveInfinity, MeasureMode.Undefined,
                                                     float.PositiveInfinity, MeasureMode.Undefined).x;

            float totalLabelWidth = textWidth + _label.resolvedStyle.marginLeft + _label.resolvedStyle.marginRight;
            float containerInnerWidth = contentRect.width;

            if (containerInnerWidth <= 0 || totalLabelWidth <= containerInnerWidth)
                return;

            float scrollRange = containerInnerWidth - totalLabelWidth;
            int waitBeforeMs = Mathf.Max(0, Mathf.RoundToInt(_waitBefore * 1000f));

            _initialWaitTask = schedule.Execute(() =>
            {
                StartLabelAnimation(0f, scrollRange, _durationMs, () =>
                {
                    _currentAnim = null;

                    if (Loop)
                    {
                        int waitAfterMs = Mathf.Max(0, Mathf.RoundToInt(_waitAfter * 1000f));
                        _postWaitTask = schedule.Execute(StartCycle).StartingIn(waitAfterMs);
                    }
                });

                _initialWaitTask = null;
            }).StartingIn(waitBeforeMs);
        }

        private void StartLabelAnimation(float from, float to, int durationMs, System.Action onComplete)
        {
            StopAnimation();

            _currentAnim = _label.experimental.animation.Start(
                from, to, durationMs, (el, val) => el.style.left = val);

            _currentAnim.easingCurve = Easing.Linear;
            _currentAnim.onAnimationCompleted = onComplete;
            _currentAnim.KeepAlive();
        }
    }
}