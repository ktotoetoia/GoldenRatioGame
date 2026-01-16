using UnityEngine;

namespace IM.Visuals
{
    public interface IModuleVisualObjectAnimated : IModuleVisualObject
    {
        Animator Animator { get; }
    }
}