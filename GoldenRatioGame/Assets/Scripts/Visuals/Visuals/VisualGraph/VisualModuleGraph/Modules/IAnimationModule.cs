using System;
using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public interface IAnimationModule : IVisualModule
    {
        Animator Animator { get; }
    }
}