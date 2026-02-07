using IM.Graphs;
using IM.Movement;
using UnityEngine;

namespace IM.Visuals
{
    public class ModuleGraphVisualObserverMono : MonoBehaviour, IModuleGraphSnapshotObserver
    {
        [SerializeField] private Transform _parent;
        private IVectorMovement _vectorMovement;
        private ModuleGraphVisualObserver _moduleGraphVisualObserver;

        private void Awake()
        {
            _moduleGraphVisualObserver = new ModuleGraphVisualObserver(_parent,true);
            _vectorMovement = GetComponent<IVectorMovement>();
        }

        private void Update()
        {
            if (_vectorMovement.MovementDirection.x != 0)
            {
                _parent.localScale =
                    _vectorMovement.MovementDirection.x > 0
                        ? Vector3.one
                        : new Vector3(-1f, 1f, 1f);
            }
            
            _moduleGraphVisualObserver.Update();
        }

        public void OnGraphUpdated(IModuleGraphReadOnly graph) => _moduleGraphVisualObserver.OnGraphUpdated(graph);
    }
}