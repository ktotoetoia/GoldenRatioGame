using IM.Graphs;
using IM.Values;
using System.Collections.Generic;

namespace IM.Modules
{
    public class SpeedExtensionsObserver
    {
        private readonly IHaveSpeed _speed;
        private readonly List<ISpeedExtension> _used = new();

        public SpeedExtensionsObserver(IHaveSpeed speed)
        {
            _speed = speed;
        }

        public void Add(IModule module)
        {
            if(module is not IExtensibleModule ext || !ext.TryGetExtension(out ISpeedExtension speedExtension) || speedExtension.SpeedModifier == null)
                return;
            
            _speed.Speed.AddModifier(speedExtension.SpeedModifier);
            _used.Add(speedExtension);
        }

        public void Remove(IModule module)
        {
            if (module is not IExtensibleModule ext || !ext.TryGetExtension(out ISpeedExtension speedExtension) || !_used.Contains(speedExtension))
                return;
            
            _speed.Speed.RemoveModifier(speedExtension.SpeedModifier);
            _used.Remove(speedExtension);
        }
    }
}