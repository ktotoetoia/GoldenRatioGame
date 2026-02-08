using System;
using IM.Movement;
using UnityEngine;

namespace IM.Visuals
{
    [Serializable]
    public class MovementDirectionEntry : IMemberValueProvider<MovementDirection>
    {
        public Component targetComponent;
        public string memberName;
        public SerializedValue left = new ();
        public SerializedValue right = new ();
            
        public Component TargetComponent => targetComponent;
        public string MemberName => memberName;
            
        public SerializedValue GetC(MovementDirection t)
        {
            return t == MovementDirection.Left ? left : right;
        }
    }
}