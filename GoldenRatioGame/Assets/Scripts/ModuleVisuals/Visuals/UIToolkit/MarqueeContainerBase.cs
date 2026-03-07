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
        
        private float _lastWidth;

        [UxmlAttribute] public EasingMode EasingMode { get; set; } = EasingMode.Linear;
        [UxmlAttribute] public float WaitBeforeSec { get; set; } = 2f;
        [UxmlAttribute] public float DurationSec { get; set; } = 3f;
        [UxmlAttribute] public float WaitAfterSec { get; set; } = 2f;
        [UxmlAttribute] public bool Loop { get; set; } = true;

        [UxmlAttribute]
        public string Text
        {
            get => TargetLabel.text;
            set
            {
                if (TargetLabel.text == value) return;
                TargetLabel.text = value ?? string.Empty;
                RequestReset(MarqueeResetReason.TextChange);
            }
        }

        public MarqueeContainerBase()
        {
            style.overflow = Overflow.Hidden;
            style.alignSelf = Align.Stretch;
            style.flexGrow = 1;

            TargetLabel = CreateLabel();
            Add(TargetLabel);
            TargetLabel.AddToClassList("marquee-container-label");

            RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            RegisterCallback<DetachFromPanelEvent>(_ => StopEverything());
        }

        protected virtual Label CreateLabel() => new Label { style = { position = Position.Absolute, whiteSpace = WhiteSpace.NoWrap } };

        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            if (Mathf.Approximately(evt.newRect.width, _lastWidth)) return;
            _lastWidth = evt.newRect.width;
            RequestReset(MarqueeResetReason.GeometryChange);
        }

        public void RequestReset(MarqueeResetReason reason)
        {
            StopMovement();
            
            OnMarqueeReset(reason);

            long delay = (reason == MarqueeResetReason.CycleEnd) ? ToMs(WaitAfterSec) : 0;
            
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
            TargetLabel.style.left = 0;
    
            OnPrepareForMove();

            if (!CalculateScrollRange(out float scrollRange))
            {
                TargetLabel.style.position = Position.Relative;

                TargetLabel.style.width = StyleKeyword.Auto; 
        
                OnMovementNotRequired();
                return;
            }

            TargetLabel.style.position = Position.Absolute;

            ScheduledTask = schedule.Execute(() => StartAnimation(scrollRange)).StartingIn(ToMs(WaitBeforeSec));
        }

        protected virtual void StartAnimation(float scrollRange)
        {
            OnAnimationStart(scrollRange);

            CurrentAnim = TargetLabel.experimental.animation.Start(
                0f, scrollRange, Mathf.RoundToInt(DurationSec * 1000), ApplyAnimationValue
            );

            CurrentAnim.easingCurve = EasingUtils.GetEasing(EasingMode);
            CurrentAnim.KeepAlive();
            CurrentAnim.onAnimationCompleted = () => {
                OnAnimationEnd();
                if (Loop) RequestReset(MarqueeResetReason.CycleEnd);
            };
        }

        protected virtual bool CalculateScrollRange(out float range)
        {
            range = 0;
            float containerWidth = contentRect.width;
            float textWidth = TargetLabel.MeasureTextSize(TargetLabel.text, 0, MeasureMode.Undefined, 0, MeasureMode.Undefined).x;
            float totalWidth = textWidth + TargetLabel.resolvedStyle.marginLeft + TargetLabel.resolvedStyle.marginRight;

            if (containerWidth <= 0 || totalWidth <= containerWidth) return false;

            range = containerWidth - totalWidth;
            return true;
        }

        protected virtual void ApplyAnimationValue(VisualElement el, float val) => el.style.left = val;

        protected virtual void OnMarqueeReset(MarqueeResetReason reason) { }
        protected virtual void OnPrepareForMove() { }
        protected virtual void OnMovementNotRequired() { }
        protected virtual void OnAnimationStart(float range) { }
        protected virtual void OnAnimationEnd() { }

        protected long ToMs(float seconds) => (long)(Mathf.Max(0, seconds) * 1000);
    }
}