using System;
using UnityEngine;

namespace IM.Modules
{
    [Serializable]
    public class PortInfo
    {
        [SerializeField] private Vector3 _position;
        [SerializeField] private Vector3 _normal;
        [SerializeField] private string _tag;
        
        public Vector3 Position => _position;
        public Vector3 Normal => _normal;
        public string Tag => _tag;
    }
}