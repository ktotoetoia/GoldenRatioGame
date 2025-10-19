using System.Linq;
using IM.Graphs;
using IM.Modules;
using UnityEngine;

namespace IM.GraphEditor
{
    public class GraphDrawer : MonoBehaviour
    {
        private IModuleEntity _entity;
        private IModuleGraphReadOnly _graph;
        
        private void Awake()
        {
            _entity = GetComponent<IModuleEntity>();
            _graph = _entity.GraphEditor.Graph;
        }

        private void OnDrawGizmos()
        {
            IGameModule coreModule = _graph.Modules.FirstOrDefault(x => x is ICoreGameModule) as IGameModule;
            
            if(coreModule == null) return;
            
        }
    }
}