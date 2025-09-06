using System;
using IM.StateMachines;
using UnityEngine;

namespace IM.Movement
{
    public class MovementState : State
    {
        private readonly IMoveInVector _movement;
        private readonly Func<Vector2> _getDirection;  
        private Vector2 _direction;
        
        public Func<bool> ShouldMove { get; set; } = () => true;
        
        public MovementState(IMoveInVector movement, Func<Vector2> getDirection)
        {
            _movement = movement;
            _getDirection = getDirection;
        }

        public override void Update()
        {
            _direction = _getDirection();
        }

        public override void FixedUpdate()
        {
            if (ShouldMove())
            {
                _movement.Move(_direction);
            }
        }
    }
}