using System;
using IM.Movement;
using UnityEngine;

namespace IM.Modules
{
    public class MovementEnumStateExtension :
        MonoBehaviour,
        IRequireMovement,
        IValueStateExtension<MovementDirection>
    {
        private Vector2 _lastVelocity;
        private MovementDirection _value;

        public MovementDirection Value
        {
            get => _value;
            set
            {
                if (_value == value) return;
                _value = value;
                ValueChanged?.Invoke(value);
            }
        }

        public event Action<MovementDirection> ValueChanged;

        public void UpdateCurrentVelocity(Vector2 velocity)
        {
            if (velocity == _lastVelocity) return;

            _lastVelocity = velocity;

            if (velocity == Vector2.zero)
            {
                Value = MovementDirection.None;
                return;
            }

            MovementDirection dir = MovementDirection.None;

            if (velocity.x > 0f) dir |= MovementDirection.Right;
            if (velocity.x < 0f) dir |= MovementDirection.Left;
            if (velocity.y > 0f) dir |= MovementDirection.Up;
            if (velocity.y < 0f) dir |= MovementDirection.Down;

            Value = dir;
        }
    }
}