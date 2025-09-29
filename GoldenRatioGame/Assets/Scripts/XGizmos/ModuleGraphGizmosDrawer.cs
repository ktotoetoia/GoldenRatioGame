using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.ModuleGraphGizmosDebug
{
    public class ModuleGraphGizmosDrawer : MonoBehaviour, IModuleGraphDrawer
    {
        [SerializeField] private KeyCode _key = KeyCode.Mouse0;
        [SerializeField] private Vector2 _moduleSize = Vector2.one;
        [SerializeField] private Vector2 _moduleStartPosition;
        private List<ModuleVisual> _modules;
        
        public List<ModuleVisual> Modules => _modules ??= new List<ModuleVisual>();
        public IModuleGraph Graph { get; set; }

        private void OnDrawGizmos()
        {
            if (Graph == null) return;
            
            UpdateModules();
            DrawModules();
        }

        private void UpdateModules()
        {
            foreach (IModule module in Graph.Modules)
            {
                if(IsWrapped(module)) continue;
                
                Modules.Add(new ModuleVisual(module,new Bounds(_moduleStartPosition, _moduleSize)));
            }
        }

        private void DrawModules()
        {
            foreach (ModuleVisual visual in Modules)
            {
                Gizmos.DrawCube(visual.Bounds.center, visual.Bounds.size);
            }
        }

        private bool IsWrapped(IModule module)
        {
            return Modules.Any(x => x.Module == module);
        }
    }
}