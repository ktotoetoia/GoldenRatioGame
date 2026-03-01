using System.Linq;
using IM.Entities;
using IM.Graphs;
using IM.Storages;
using UnityEngine;

namespace IM.Modules
{
    [DisallowMultipleComponent]
    public class ModuleEntity : DefaultEntity, IModuleEntity, IRequireInteractionProvider
    {
        [SerializeField] private GameObject _observersSource;
        private IModuleEditingContext _moduleEditingContext;
        
        public IModuleEditingContext ModuleEditingContext
        {
            get
            {
                if (_moduleEditingContext != null) return _moduleEditingContext;
                
                _moduleEditingContext = new ModuleEditingContext(new CellFactoryStorage());
                
                foreach (IModuleGraphSnapshotObserver observer in _observersSource.GetComponents<IModuleGraphSnapshotObserver>())
                {
                    ModuleEditingContext.GraphEditor.Observers.Add(observer);
                }

                return _moduleEditingContext;
            }
        }

        public IInteractionProvider InteractionProvider { get; set; }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                InteractionProvider.GetAvailableInteractions(this).OrderBy(x => Vector3.Distance(transform.position, x.GameObject.transform.position)).FirstOrDefault()?.Interact(this);
            }
        }
    }
}