using System;
using UnityEngine;

namespace IM.Modules
{
    [Serializable]
    public class PortPositionRotation
    {
        [field: SerializeField] public Vector3 Position { get; private set; }
        [field: SerializeField] public float EulerZRotation { get; private set; }
    }
}