using System;
using IM.Items;
using UnityEngine;

namespace IM.Modules
{
    [Serializable]
    public class TagDirectionPortInfo : IPortInfo
    {
        [SerializeField] private Vector3 _position;
        [SerializeField] private float _eulerZRotation;
        [SerializeField] private LazyTag _tag;
        [SerializeField] private HorizontalDirection _direction;
        
        public Vector3 Position => _position;
        public float EulerZRotation => _eulerZRotation;
        public ITag Tag => (ITag)_tag ?? new EmptyTag();
        public HorizontalDirection Direction => _direction;
    }
}