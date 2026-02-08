using System;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    [Serializable]
    public class PortDirectionEntry : IMemberValueProvider<PortDirection>
    {
        public Component targetComponent;
        public string memberName;
        public SerializedValue left = new ();
        public SerializedValue right = new ();
        
        public Component TargetComponent => targetComponent;
        public string MemberName => memberName;
        
        public SerializedValue GetT(PortDirection t)
        {
            return t == PortDirection.Left ? left : right;
        }
    }
}