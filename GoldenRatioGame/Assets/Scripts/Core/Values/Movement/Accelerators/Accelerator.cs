using UnityEngine;

namespace IM.Values
{
    public class Accelerator : IAccelerator
    {
        private readonly ICappedValue<float> _x = new CappedValue<float>(-1f, 1f);
        private readonly ICappedValue<float> _y = new CappedValue<float>(-1f, 1f);

        public float AccelerationTime { get; set; } = 0.2f;

        public Vector2 Acceleration => new(_x.Value, _y.Value);

        public void Update(Vector2 direction, float deltaTime)
        {
            float step = (AccelerationTime > 0f) ? (deltaTime / AccelerationTime) : 1f;

            _x.Value = UpdateAxis(_x.Value, direction.x, step);
            _y.Value = UpdateAxis(_y.Value, direction.y, step);
        }
        
        private float UpdateAxis(float current, float input, float step)
        {
            if (input > 0)
            {
                current += step;
            }
            else if (input < 0)
            {
                current -= step;
            }
            else
            {
                if (current > 0)
                    current = Mathf.Max(0, current - step);
                else if (current < 0)
                    current = Mathf.Min(0, current + step);
            }

            return current;
        }

        public void Reset()
        {
            _x.Value = 0f;
            _y.Value = 0f;
        }
    }
}