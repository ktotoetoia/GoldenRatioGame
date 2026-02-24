using System;
using IM.StateMachines;
using UnityEngine;

namespace IM.Movement
{
    public class MovementState : State
    {
        private readonly IMoveInVector _movement;
        private readonly Func<Vector2> _getDirection;  
        
        public Func<bool> ShouldMove { get; set; } = () => true;
        
        public MovementState(IMoveInVector movement, Func<Vector2> getDirection)
        {
            _movement = movement;
            _getDirection = getDirection;
        }

        public override void Update()
        {
            if(ShouldMove()) _movement.Move(_getDirection());
        }
        public override void OnExit()
        {
            _movement.Move(Vector2.zero);
        }
    }
}