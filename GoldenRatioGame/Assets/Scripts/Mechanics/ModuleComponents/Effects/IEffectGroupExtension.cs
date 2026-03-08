using IM.Effects;
using UnityEngine;

namespace IM.Modules
{
    public interface IEffectGroupExtension : IExtension
    {
        IEffectGroup EffectGroup { get; }
    }
}