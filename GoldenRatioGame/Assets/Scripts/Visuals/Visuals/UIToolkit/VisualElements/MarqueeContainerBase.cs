using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace IM.Visuals
{
    public enum MarqueeResetReason
    {
        GeometryChange,
        TextChange,
        CycleEnd
    }

    [UxmlElement]
    public partial class MarqueeContainerBase : VisualElement
    {
        protected readonly Label TargetLabel;
        protected ValueAnimation<float> CurrentAnim;
        protected IVisualElementScheduledItem ScheduledTask;

        private float _lastContainerWidth = -1f;

        [UxmlAttribute] public EasingMode EasingMode  { get; set; } = EasingMode.Linear;
        [UxmlAttribute] public float WaitBeforeSec     { get; set; } = 2f;
        [UxmlAttribute] public float DurationSec       { get; set; } = 3f;
        [UxmlAttribute] public float WaitAfterSec      { get; set; } = 2f;
        [UxmlAttribute] public bool  Loop              { get; set; } = true;

        [UxmlAttribute]
        public string Text
        {
            get => TargetLabel.text;
            set
            {
                value ??= string.Empty;
                if (TargetLabel.text == value) return;
                TargetLabel.text = value;
                RequestReset(MarqueeResetReason.TextChange);
            }
        }

        public MarqueeContainerBase()
        {
            style.overflow  = Overflow.Hidden;
            style.alignSelf = Align.Stretch;
            style.flexGrow  = 1;

            TargetLabel = CreateLabel();
            TargetLabel.AddToClassList("marquee-container-label");
            Add(TargetLabel);

            RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        protected virtual Label CreateLabel() => new Label
        {
            style =
            {
                position  = Position.Relative,
                whiteSpace = WhiteSpace.NoWrap,
                flexShrink = 0,
                alignSelf  = Align.FlexStart
            }
        };

        public void RequestReset(MarqueeResetReason reason)
        {
            StopMovement();
            OnMarqueeReset(reason);

            long delay = reason == MarqueeResetReason.CycleEnd
                ? ToMs(WaitAfterSec)
                : 0L;

            ScheduledTask = schedule.Execute(RunMarqueeLogic).StartingIn(delay);
        }

        protected virtual void StopMovement()
        {
            if (CurrentAnim != null)
            {
                CurrentAnim.onAnimationCompleted = null;
                CurrentAnim.Stop();
                CurrentAnim = null;
            }

            ScheduledTask?.Pause();
            ScheduledTask = null;
        }

        protected virtual void StopEverything()
        {
            StopMovement();
            TargetLabel.style.left = 0;
        }

        private void RunMarqueeLogic()
        {
            ScheduledTask = null;

            TargetLabel.style.left = 0;

            OnPrepareForMove();

            if (!CalculateScrollRange(out float scrollRange))
            {
                OnMovementNotRequired();
                return;
            }

            ScheduledTask = schedule
                .Execute(() =>
                {
                    ScheduledTask = null;
                    StartAnimation(scrollRange);
                })
                .StartingIn(ToMs(WaitBeforeSec));
        }

        protected virtual void StartAnimation(float scrollRange)
        {
            OnAnimationStart(scrollRange);

            int durationMs = Mathf.Max(1, Mathf.RoundToInt(DurationSec * 1000f));

            CurrentAnim = TargetLabel.experimental.animation
                .Start(0f, scrollRange, durationMs, ApplyAnimationValue);

            CurrentAnim.easingCurve = EasingUtils.GetEasing(EasingMode);
            CurrentAnim.KeepAlive();

            CurrentAnim.onAnimationCompleted = () =>
            {
                CurrentAnim = null;
                OnAnimationEnd();
                if (Loop) RequestReset(MarqueeResetReason.CycleEnd);
            };
        }

        protected virtual bool CalculateScrollRange(out float range)
        {
            range = 0f;

            float containerWidth = contentRect.width;
            if (containerWidth <= 0f) return false;

            float textWidth = TargetLabel.MeasureTextSize(
                TargetLabel.text,
                0, MeasureMode.Undefined,
                0, MeasureMode.Undefined).x;

            if (textWidth <= 0f) return false;

            float marginLeft  = TargetLabel.resolvedStyle.marginLeft;
            float marginRight = TargetLabel.resolvedStyle.marginRight;
            float totalWidth  = textWidth + marginLeft + marginRight;

            if (totalWidth <= containerWidth) return false;

            range = containerWidth - totalWidth;
            return true;
        }

        protected virtual void ApplyAnimationValue(VisualElement element, float value)
            => element.style.left = value;

        protected virtual void OnMarqueeReset(MarqueeResetReason reason) { }
        protected virtual void OnPrepareForMove()                        { }
        protected virtual void OnMovementNotRequired()                   { }
        protected virtual void OnAnimationStart(float range)             { }
        protected virtual void OnAnimationEnd()                          { }

        private void OnAttachToPanel(AttachToPanelEvent evt)
        {
            _lastContainerWidth = -1f;

            schedule.Execute(() => RequestReset(MarqueeResetReason.GeometryChange));
        }

        private void OnDetachFromPanel(DetachFromPanelEvent evt)
        {
            _lastContainerWidth = -1f;
            StopEverything();
        }

        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            float width = evt.newRect.width;

            if (width <= 0f || Mathf.Approximately(width, _lastContainerWidth)) return;

            _lastContainerWidth = width;
            RequestReset(MarqueeResetReason.GeometryChange);
        }

        protected long ToMs(float seconds) => (long)(Mathf.Max(0f, seconds) * 1000f);
    }
}