using System;
using System.Reflection;
using UnityEngine;

namespace IM.Visuals
{
    [Serializable]
    public class TargetEntry
    {
        public Component targetComponent;
        public string memberName;

        public SerializedValue left = new ();
        public SerializedValue right = new ();
    }
}