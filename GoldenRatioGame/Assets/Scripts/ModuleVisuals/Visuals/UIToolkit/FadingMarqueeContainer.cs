using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace IM.Visuals
{
    [UxmlElement]
    public sealed partial class FadingMarqueeContainer : MarqueeContainerBase
    {
        private ValueAnimation<float> _fadeAnimation;
        [UxmlAttribute] public EasingMode FadingEasingMode { get; set; } = EasingMode.Linear;
        [UxmlAttribute] public float FadeDurationSec { get; set; } = 0.5f;

        protected override void StopEverything()
        {
            base.StopEverything();
            _fadeAnimation?.Stop();
            _fadeAnimation = null;
            TargetLabel.style.opacity = 1f;
        }
        
        protected override void OnMarqueeReset(MarqueeResetReason reason)
        {
            Debug.Log(reason.ToString());
            if (reason == MarqueeResetReason.CycleEnd)
            {
                schedule.Execute(() => StartFade(1f, 0f)).StartingIn((long)((WaitAfterSec - FadeDurationSec-.2) * 1000));
            }
            else
            {
                _fadeAnimation?.Stop();
                TargetLabel.style.opacity = 1f;
            }
        }
        
        protected override void OnPrepareForMove()
        {
            StartFade(TargetLabel.style.opacity.value, 1f);
        }

        private void StartFade(float from, float to)
        {
            _fadeAnimation?.Stop();

            float actualDuration = Mathf.Min(FadeDurationSec, from > to ? WaitAfterSec : WaitBeforeSec);
            
            _fadeAnimation = TargetLabel.experimental.animation.Start(
                from, 
                to, 
                Mathf.RoundToInt(actualDuration * 1000), 
                (el, val) => el.style.opacity = val
            );
            
            _fadeAnimation.easingCurve = EasingUtils.GetEasing(FadingEasingMode);
            _fadeAnimation.KeepAlive();
        }
    }
}