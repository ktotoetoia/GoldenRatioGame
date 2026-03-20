using System;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace IM.Visuals
{
    public static class EasingUtils
    {
        public static Func<float, float> GetEasing(EasingMode mode)
        {
            return mode switch
            {
                EasingMode.Ease => Easing.InOutQuad,
                EasingMode.EaseIn => Easing.InQuad,
                EasingMode.EaseOut => Easing.OutQuad,
                EasingMode.EaseInOut => Easing.InOutQuad,

                EasingMode.Linear => Easing.Linear,

                EasingMode.EaseInSine => Easing.InSine,
                EasingMode.EaseOutSine => Easing.OutSine,
                EasingMode.EaseInOutSine => Easing.InOutSine,

                EasingMode.EaseInCubic => Easing.InCubic,
                EasingMode.EaseOutCubic => Easing.OutCubic,
                EasingMode.EaseInOutCubic => Easing.InOutCubic,
                
                EasingMode.EaseInCirc => Easing.InCirc,
                EasingMode.EaseOutCirc => Easing.OutCirc,
                EasingMode.EaseInOutCirc => Easing.InOutCirc,
                
                EasingMode.EaseInElastic => Easing.InElastic,
                EasingMode.EaseOutElastic => Easing.OutElastic,
                EasingMode.EaseInOutElastic => Easing.InOutElastic,
                
                EasingMode.EaseInBack => Easing.InBack,
                EasingMode.EaseOutBack => Easing.OutBack,
                EasingMode.EaseInOutBack => Easing.InOutBack,
                
                EasingMode.EaseInBounce => Easing.InBounce,
                EasingMode.EaseOutBounce => Easing.OutBounce,
                EasingMode.EaseInOutBounce => Easing.InOutBounce,
                
                _ => Easing.Linear
            };
        }
    }
}