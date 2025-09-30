using System.Collections.Generic;
using System.Linq;
using IM.Base;
using IM.Graphs;
using UnityEngine;

namespace IM.ModuleGraphGizmosDebug
{
    public class ModuleGraphGizmosDrawer : MonoBehaviour, IModuleGraphDrawer
    {
        [SerializeField] private Vector2 _moduleSize = Vector2.one;
        [SerializeField] private Vector2 _moduleStartPosition;
        private IModuleGraph _graph;
        private EnumerableWrapper<IModule,ModuleWrapper> _moduleEnumerableWrappers;

        public IEnumerable<IModuleVisualWrapper> Modules =>  _moduleEnumerableWrappers ?? Enumerable.Empty<IModuleVisualWrapper>();

        public IModuleGraph Graph
        {
            get => _graph;
            set
            {
                _graph = value;
                _moduleEnumerableWrappers = new EnumerableWrapper<IModule, ModuleWrapper>(_graph.Modules, new ModuleWrapperFactory(_moduleStartPosition, _moduleSize));
            }
        }

        private void OnDrawGizmos()
        {
            if (Graph == null) return;
            
            DrawModules();
        }
        
        private void DrawModules()
        {
            foreach (ModuleWrapper module in Modules)
            {
                Gizmos.color = new Color(251f/ 255f,195f/ 255f,74f/ 255f);
                module.Visual.DrawGizmo();
                
                Gizmos.color = new Color( 64f/ 255f,62f/ 255f,68f/ 255f);
                foreach (IPortVisualWrapper port in module.Ports)
                {
                    port.Visual.DrawGizmo();
                }
            }
        }
    }
}