using System.Linq;
using IM.Abilities;
using IM.Entities;
using IM.Graphs;
using IM.Storages;
using UnityEngine;

namespace IM.Modules
{
    [DisallowMultipleComponent]
    public class ModuleEntity : DefaultEntity, IModuleEntity, IRequireInteractionProvider
    {
        private IModuleEditingContext _moduleEditingContext;
        
        public IAbilityPool AbilityPool { get; } = new AbilityPool();
        public IModuleEditingContext ModuleEditingContext
        {
            get
            {
                if (_moduleEditingContext == null)
                {
                    _moduleEditingContext = new ModuleEditingContext(new CellFactoryStorage());
                    foreach (IModuleGraphSnapshotObserver observer in GetComponents<IModuleGraphSnapshotObserver>())
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