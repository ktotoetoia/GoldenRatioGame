using System;
using IM.Values;
using UnityEngine;

namespace IM.Visuals
{
    [Serializable]
    public class HorizontalDirectionEntry : IMemberValueProvider<Direction>
    {
        private SerializedValue _lastReturned;
        
        public Component targetComponent;
        public string memberName;
        public SerializedValue left = new ();
        public SerializedValue right = new ();
            
        public Component TargetComponent => targetComponent;
        public string MemberName => memberName;
        
        public SerializedValue GetT(Direction t)
        {
            if((t & Direction.Left) != 0) return _lastReturned = left;
            if((t & Direction.Right) != 0) return _lastReturned = right;

            return _lastReturned ??= left;
        }
    }
}