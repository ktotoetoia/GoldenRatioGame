using IM.Base;
using IM.Graphs;
using UnityEngine;

namespace IM.ModuleGraphGizmosDebug
{
    public class ModuleWrapperFactory : IFactory<ModuleWrapper, IModule>
    {
        public  Vector3 _visualSize { get; set; }
        public  Vector3 _visualPosition { get; set; }

        public ModuleWrapperFactory(Vector3 visualPosition, Vector3 visualSize)
        {
            _visualSize = visualSize;
            _visualPosition = visualPosition;
        }
        
        public ModuleWrapper Create(IModule param1)
        {
            return new ModuleWrapper(param1, new BoundsVisual(_visualPosition, _visualSize));
        }
    }
}