using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public interface IAnimationModule : INameModule
    {
        Animator Animator { get; }
    }

    public interface INameModule : ITransformModule
    {
        IModuleTransformChanger TransformChanger { get; set; }
    }

    public interface IModuleTransformChanger
    {
        void TranslatePort(ITransformPort port, Vector3 displacement);
        void RotatePort(ITransformPort port, float zRotation);
    }
}