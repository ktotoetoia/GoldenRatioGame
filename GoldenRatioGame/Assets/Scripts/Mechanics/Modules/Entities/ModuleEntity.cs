using System;
using System.Linq;
using IM.Abilities;
using IM.Graphs;
using IM.Storages;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEntity : MonoBehaviour, IModuleEntity
    {
        public GameObject GameObject => gameObject;
        private IModuleController _moduleController;

        public IAbilityPool AbilityPool { get; } = new AbilityPool();
        public IModuleController ModuleController
        {
            get
            {
                if (_moduleController == null)
                {
                    _moduleController = new ModuleController(new CellFactoryStorage());
                    foreach (IModuleGraphSnapshotObserver observer in GetComponents<IModuleGraphSnapshotObserver>())
                        ModuleController.GraphEditor.Observers.Add(observer);
                }

                return _moduleController;
            }
        }
        
        public void SetCoreModule(ICoreExtensibleModule coreModule)
        {
            if (ModuleController.GraphEditor.Graph.Modules.Any()) throw new InvalidOperationException("Graph must be clear before setting coreModule");
            
            ModuleController.AddToStorage(coreModule);
            ICommandModuleGraph f = ModuleController.GraphEditor.StartEditing();
            f.AddModule(coreModule);
            ModuleController.GraphEditor.TrySaveChanges();
        }
    }
}