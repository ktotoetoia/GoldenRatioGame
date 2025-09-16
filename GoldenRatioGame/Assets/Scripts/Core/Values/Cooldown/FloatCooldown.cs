using UnityEngine;

namespace IM.Values
{
    public class FloatCooldown : IFloatCooldown
    {
        private float _lastTimeUsed;
        public bool IsOnCooldown => RemainingCooldown > 0;

        public float MinValue => 0;
        public float MaxValue { get; set; }
        public float Value => Mathf.Max(0f, MaxValue - (Time.time - _lastTimeUsed));
        public float TotalCooldown => MaxValue;
        public float RemainingCooldown => Value;

        public FloatCooldown(float maxValue)
        {
            MaxValue = maxValue;
        }
        
        public void ForceReset()
        {
            _lastTimeUsed = Time.time;
        }

        public bool TryReset()
        {
            if (!IsOnCooldown)
            {
                ForceReset();
                
                return true;
            }

            return false;
        }
    }
}