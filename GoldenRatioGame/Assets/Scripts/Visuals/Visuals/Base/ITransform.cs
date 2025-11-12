using UnityEngine;

namespace IM.Visuals
{
    public interface ITransform : IHavePosition,  IHaveScale, IHaveRotation
    {
        new Vector3 Position { get; set; }
        new Vector3 Scale { get; set; }
        new Quaternion Rotation { get; set; }
        public Vector3 LocalPosition { get; set; }
        public Vector3 LocalScale { get; set; }
        public Quaternion LocalRotation { get; set; }
    }

    public interface IHavePosition
    {
        Vector3 Position { get; }
    }

    public interface IHaveScale
    {
        Vector3 Scale { get; }
    }

    public interface IHaveRotation
    {
        Quaternion Rotation { get; }
    }
}