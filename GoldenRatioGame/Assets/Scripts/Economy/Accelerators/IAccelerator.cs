using UnityEngine;

namespace IM.Economy
{
    public interface IAccelerator
    {
        public float AccelerationTime { get; set; }

        public Vector2 Acceleration { get; }
        void Update(Vector2 direction, float deltaTime);
    }
}