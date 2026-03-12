using UnityEngine;

namespace IM.SaveSystem
{
    public class TransformSerializer : ComponentSerializer<Transform>
    {
        public override object CaptureState(Transform t)
        {
            return new TransformState
            {
                Position = t.localPosition,
                Rotation = t.localRotation,
                Scale = t.localScale
            };
        }

        public override void RestoreState(Transform t, object state)
        {
            var s = (TransformState)state;
            t.localPosition = s.Position;
            t.localRotation = s.Rotation;
            t.localScale = s.Scale;
        }
    }
}