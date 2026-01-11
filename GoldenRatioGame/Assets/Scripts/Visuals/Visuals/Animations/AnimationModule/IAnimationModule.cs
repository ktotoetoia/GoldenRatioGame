using UnityEngine;

namespace IM.Visuals
{
    public interface IAnimationModule : IPortTransformModule
    {
        Animator Animator { get; }
    }

    public interface IPortTransformModule : ITransformModule
    {
        IPortTransformChanger TransformChanger { get; set; }
    }

    public interface IPortTransformChanger
    {
        void SetPortPosition(ITransformPort port, Vector3 newPosition);
        void SetPortRotation(ITransformPort port, float zRotation);
    }
}