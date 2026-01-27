using IM.Abilities;
using IM.Graphs;
using IM.Storages;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEntity : MonoBehaviour, IModuleEntity
    {
        public GameObject GameObject => gameObject;
        public IModuleController ModuleController { get; private set; }
        public IAbilityPool AbilityPool { get; } = new AbilityPool();

        public void Initialize(IExtensibleModule coreModule)
        {
            ModuleController = new ModuleController(new CellFactoryStorage());
            
            foreach (IModuleGraphSnapshotObserver observer in GetComponents<IModuleGraphSnapshotObserver>())
                ModuleController.GraphEditor.Observers.Add(observer);
            Debug.Log("initialzied");
            ModuleController.AddToStorage(coreModule);
            ICommandModuleGraph f = ModuleController.GraphEditor.StartEditing();
            f.AddModule(coreModule);
            ModuleController.GraphEditor.TrySaveChanges();
        }
    }
}