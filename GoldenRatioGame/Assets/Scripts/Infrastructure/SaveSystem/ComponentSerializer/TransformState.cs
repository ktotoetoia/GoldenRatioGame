using System;
using UnityEngine;

namespace IM.SaveSystem
{
    [Serializable]
    public class TransformState
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;
    }
}