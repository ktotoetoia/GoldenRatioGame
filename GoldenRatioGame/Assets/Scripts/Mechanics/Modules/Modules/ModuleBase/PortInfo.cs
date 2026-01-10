using System;
using UnityEngine;

namespace IM.Modules
{
    [Serializable]
    public class PortInfo
    {
        [SerializeField] private Vector3 _position;
        [SerializeField] private float _eulerZRotation;
        [SerializeField] private LazyTag _tag;
        
        public Vector3 Position => _position;
        public float EulerZRotation => _eulerZRotation;
        public ITag Tag => (ITag)_tag ?? new EmptyTag();
    }
}