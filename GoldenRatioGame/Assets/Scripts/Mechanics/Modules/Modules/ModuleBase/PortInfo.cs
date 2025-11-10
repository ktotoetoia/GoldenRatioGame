using System;
using UnityEngine;

namespace IM.Modules
{
    [Serializable]
    public class PortInfo
    {
        [SerializeField] private Vector3 _position;
        [SerializeField] private Vector3 _normal;
        
        public Vector3 Position => _position;
        public Vector3 Normal => _normal;
    }
}