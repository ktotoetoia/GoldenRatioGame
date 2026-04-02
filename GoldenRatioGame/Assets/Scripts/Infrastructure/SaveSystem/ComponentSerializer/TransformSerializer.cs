using System;
using UnityEngine;

namespace IM.SaveSystem
{
    public class TransformSerializer : ComponentSerializer<Transform>
    {
        public override object CaptureState(Transform t)
        {
            return new TransformState
            {
                Position = t.position,
                Rotation = t.rotation,
                Scale = t.localScale
            };
        }

        public override void RestoreState(Transform t, object state, Func<string, GameObject> resolveDependency)
        {
            var s = (TransformState)state;
            t.position = s.Position;
            t.rotation = s.Rotation;
            t.localScale = s.Scale;
        }
        
        [Serializable]
        public class TransformState
        {
            public Vector3 Position;
            public Quaternion Rotation;
            public Vector3 Scale;
        }
    }
}