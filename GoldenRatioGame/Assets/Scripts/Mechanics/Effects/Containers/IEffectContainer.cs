using System.Collections.Generic;

namespace IM.Effects
{
    public interface IEffectContainer
    {
        IEnumerable<IEffectGroup> Groups { get; }

        void AddGroup(IEffectGroup group);
        void RemoveGroup(IEffectGroup group);
        
        IEnumerable<T> GetModifiers<T>();
    }
}