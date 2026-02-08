using System;
using IM.Movement;
using UnityEngine;

namespace IM.Visuals
{
    [Serializable]
    public class MovementDirectionEntry : IMemberValueProvider<MovementDirection>
    {
        private SerializedValue _lastReturned;
        
        public Component targetComponent;
        public string memberName;
        public SerializedValue left = new ();
        public SerializedValue right = new ();
            
        public Component TargetComponent => targetComponent;
        public string MemberName => memberName;
        
        public SerializedValue GetT(MovementDirection t)
        {
            if((t & MovementDirection.Left) != 0) return _lastReturned = left;
            if((t & MovementDirection.Right) != 0) return _lastReturned = right;

            return _lastReturned ??= left;
        }
    }
}