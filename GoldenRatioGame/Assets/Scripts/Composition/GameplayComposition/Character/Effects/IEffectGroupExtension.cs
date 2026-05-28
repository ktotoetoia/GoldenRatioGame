using IM.Modules;

namespace IM.Effects
{
    public interface IEffectGroupExtension : IExtension
    {
        IEffectGroup EffectGroup { get; }
    }
}