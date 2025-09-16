using IM.Graphs;
using IM.Modules;
using UnityEngine;

namespace IM.ModuleEditor
{
    [DefaultExecutionOrder(1000)]
    public class GizmosConnector : MonoBehaviour
    {
        [SerializeField] private Vector2 _position;
        [SerializeField] private Vector2 _size = Vector2.one;
        [SerializeField] private ModuleGraphGizmos _gizmos;
        [SerializeField] private ModuleEntity _entity;
        [SerializeField] private ModuleGraphEditor _graphEditor;
        private PositionModuleGraph _positionModuleGraph;
        
        private void Awake()
        {
            _positionModuleGraph = new PositionModuleGraph(_entity.Graph as IModuleGraphEvents);
            
            _gizmos.Graph = _positionModuleGraph;
            _graphEditor.ModuleGraph =  _positionModuleGraph;
        }

        private void Update()
        {
            _positionModuleGraph.NewModulePosition = _position;
            _positionModuleGraph.NewModuleSize = _size;
        }
    }
}