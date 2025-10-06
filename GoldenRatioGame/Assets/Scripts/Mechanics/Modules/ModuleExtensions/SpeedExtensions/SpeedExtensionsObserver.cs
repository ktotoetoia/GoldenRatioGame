using System;
using IM.Graphs;
using IM.Values;
using System.Collections.Generic;
using System.Linq;

namespace IM.Modules
{
    public class SpeedExtensionsObserver : IModuleGraphObserver
    {
        private readonly IHaveSpeed _speed;
        private readonly HashSet<ISpeedExtension> _usedSpeedModules = new();

        public SpeedExtensionsObserver(IHaveSpeed speed)
        {
            _speed = speed ?? throw new ArgumentNullException(nameof(speed));
        }

        public void Update(IModuleGraphReadOnly graph)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));

            HashSet<ISpeedExtension> currentSpeedModules = new();

            foreach (IModule module in graph.Modules)
            {
                if (module is IExtensibleModule ext &&
                    ext.TryGetExtension(out ISpeedExtension speedExt) &&
                    speedExt.SpeedModifier != null)
                {
                    currentSpeedModules.Add(speedExt);
                }
            }
            
            foreach (ISpeedExtension speedExt in currentSpeedModules)
            {
                if (_usedSpeedModules.Add(speedExt))
                {
                    _speed.Speed.AddModifier(speedExt.SpeedModifier);
                }
            }
            
            foreach (ISpeedExtension speedExt in _usedSpeedModules.Except(currentSpeedModules).ToList())
            {
                _usedSpeedModules.Remove(speedExt);
                _speed.Speed.RemoveModifier(speedExt.SpeedModifier);
            }
        }
    }
}