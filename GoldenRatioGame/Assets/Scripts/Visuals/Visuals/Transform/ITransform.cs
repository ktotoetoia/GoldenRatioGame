using UnityEngine;

namespace IM.Visuals
{
    public interface ITransform : ITransformReadOnly
    {
        new Vector3 Position { get; set; }
        new Vector3 LossyScale { get; }
        new Quaternion Rotation { get; set; }
        new Vector3 LocalPosition { get; set; }
        new Vector3 LocalScale { get; set; }
        new Quaternion LocalRotation { get; set; }
    }
}