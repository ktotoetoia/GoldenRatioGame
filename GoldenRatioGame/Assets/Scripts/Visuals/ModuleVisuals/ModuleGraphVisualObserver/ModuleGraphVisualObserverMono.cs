using IM.Graphs;
using IM.Movement;
using IM.Transforms;
using UnityEngine;

namespace IM.Visuals
{
    public class ModuleGraphVisualObserverMono : MonoBehaviour, IModuleGraphSnapshotObserver
    {
        private readonly IHierarchyTransform _transform = new HierarchyTransform();
        private IVectorMovement _vectorMovement;
        private ModuleGraphVisualObserver _moduleGraphVisualObserver;

        private void Awake()
        {
            _moduleGraphVisualObserver = new ModuleGraphVisualObserver(_transform);
            _vectorMovement = GetComponent<IVectorMovement>();
        }

        private void Update()
        {
            if (_vectorMovement.MovementDirection.x != 0)
            {
                _transform.LocalScale =
                    _vectorMovement.MovementDirection.x > 0
                        ? Vector3.one
                        : new Vector3(-1f, 1f, 1f);
            }
            
            
            _transform.Position = transform.position;
        }

        public void OnGraphUpdated(IModuleGraphReadOnly graph) => _moduleGraphVisualObserver.OnGraphUpdated(graph);
    }
}