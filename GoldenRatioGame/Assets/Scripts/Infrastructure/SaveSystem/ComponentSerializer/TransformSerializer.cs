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

        public override void RestoreState(Transform t, object state)
        {
            var s = (TransformState)state;
            t.position = s.Position;
            t.rotation = s.Rotation;
            t.localScale = s.Scale;
        }
    }
}