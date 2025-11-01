using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Modules;
using UnityEngine;

namespace IM.ModuleGraphGizmosDebug
{
    public class ModuleGraphGizmosDrawer : MonoBehaviour, IModuleGraphVisual
    {
        [SerializeField] private float _portSize = 0.1f;

        public IModuleGraphReadOnly Source { get; private set; }
        private ICoreGameModule _coreModule;
        private IReadOnlyDictionary<IGameModule, DecoratedModule> _decoratedModules;

        public void SetSource(IModuleGraphReadOnly source, ICoreGameModule coreModule)
        {
            Source = source;
            _coreModule = coreModule;
            Rebuild();
        }

        private void Rebuild()
        {
            if (_coreModule == null) { _decoratedModules = null; return; }
            DecoratedGraphBuilder builder = new DecoratedGraphBuilder();
            _decoratedModules = builder.Build(_coreModule);
        }
        
        private void OnDrawGizmos()
        {
            if (_coreModule == null) return;
            if (_decoratedModules == null) Rebuild();
            if (_decoratedModules == null) return;

            foreach (DecoratedModule decorated in _decoratedModules.Values)
            {
                DrawModuleGizmo(decorated);
            }
        }

        private void DrawModuleGizmo(DecoratedModule d)
        {
            Matrix4x4 oldMatrix = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(d.Position, d.Rotation, Vector3.one);
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one * 0.9f);
            Gizmos.matrix = oldMatrix;

            foreach (IPortLayout layout in d.Module.ModuleLayout.PortLayouts)
            {
                Vector3 world = d.GetPortWorldPosition(layout);
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(world, _portSize);
            }
        }
    }
}