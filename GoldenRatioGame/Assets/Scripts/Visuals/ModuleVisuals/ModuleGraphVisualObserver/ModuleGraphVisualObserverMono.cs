using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public class ModuleGraphVisualObserverMono : MonoBehaviour, IModuleGraphSnapshotObserver
    {
        [SerializeField] private Transform _parent;
        private ModuleGraphVisualObserver _moduleGraphVisualObserver;

        private void Awake()
        {
            _moduleGraphVisualObserver = new ModuleGraphVisualObserver(_parent,true); 
        }

        private void Update()
        {
            _moduleGraphVisualObserver.Update();    
        }

        public void OnGraphUpdated(IModuleGraphReadOnly graph) 
        {
            _moduleGraphVisualObserver.OnGraphUpdated(graph);
            _moduleGraphVisualObserver.Update();    
        }
    }
}