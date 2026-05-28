using UnityEngine;

namespace IM.Effects
{
    public interface IEffectContext
    {
        GameObject Instigator { get; }
        GameObject Target { get; }
    }
}