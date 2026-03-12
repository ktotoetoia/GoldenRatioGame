using System.Collections.Generic;
using System.Linq;

namespace IM.Effects
{
    public class EffectContainer : IEffectContainer
    {
        private readonly HashSet<IEffectGroup> _groups = new();

        public IEnumerable<IEffectGroup> Groups => _groups;
        
        public void AddGroup(IEffectGroup group)
        {
            _groups.Add(group);
        }

        public void RemoveGroup(IEffectGroup group)
        {
            _groups.Remove(group);
        }

        public IEnumerable<T> GetModifiers<T>()
        {
            return _groups.SelectMany(x => x.Modifiers.GetAll<T>());
        }
    }
}