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
        [SerializeField] private bool _off;
        private IModuleGraphReadOnly _graph;
        private EnumerableWrapper<IModule,IModuleVisualWrapper> _modules;

        public IEnumerable<IModuleVisualWrapper> Modules =>  _modules ?? Enumerable.Empty<IModuleVisualWrapper>();

        public IModuleGraphReadOnly Graph
        {
            get => _graph;
            set
            {
                _graph = value;
                _modules = new EnumerableWrapper<IModule, IModuleVisualWrapper>(_graph.Modules, new ModuleWrapperFactory(_moduleStartPosition, _moduleSize));
            }
        }

        private void OnDrawGizmos()
        {
            if (Graph == null || _off) return;
            
            DrawModules();
            DrawConnections();
        }
        
        private void DrawModules()
        {
            foreach (IModuleVisualWrapper module in Modules)
            {
                Gizmos.color = new Color(251f/ 255f,195f/ 255f,74f/ 255f);
                module.Visual.DrawGizmos();
                
                Gizmos.color = new Color( 64f/ 255f,62f/ 255f,68f/ 255f);
                foreach (IPortVisualWrapper port in module.Ports)
                {
                    port.Visual.DrawGizmos();
                }
            }
        }

        private void DrawConnections()
        {
            foreach (IConnection connection in Graph.Connections)
            {
                IPortVisualWrapper output = _modules.Cache[connection.Output.Module].Ports.First(x => x.Port == connection.Output);
                IPortVisualWrapper input = _modules.Cache[connection.Input.Module].Ports.First(x => x.Port == connection.Input);
                
                Gizmos.DrawLine(output.Visual.Position, input.Visual.Position);
            }
        }
    }
}