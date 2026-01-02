using UnityEngine;

namespace IM.Visuals
{
    public interface IAnimationModule : ITransformModule
    {
        Animator Animator { get; }
    }
}