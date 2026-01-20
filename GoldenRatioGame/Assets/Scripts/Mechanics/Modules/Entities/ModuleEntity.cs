using IM.Abilities;
using IM.Graphs;
using IM.Storages;
using UnityEngine;

namespace IM.Modules
{
    [DefaultExecutionOrder(10)]
    public class ModuleEntity : MonoBehaviour, IModuleEntity
    {
        [SerializeField] private GameObject _coreModulePrefab;

        public GameObject GameObject => gameObject;
        public IModuleGraphEditor<IConditionalCommandModuleGraph> GraphEditor { get; private set; }
        public IAbilityPool AbilityPool { get; private set; }

        private void Awake()
        {
            AbilityPool = new AbilityPool();
            ConditionalCommandModuleGraph graph = new ConditionalCommandModuleGraph(new CommandModuleGraph(), new PortTagsModuleGraphConditions());
            GraphEditor = new CommandModuleGraphEditor<IConditionalCommandModuleGraph>(graph,new AccessConditionalCommandModuleGraphFactory());
            
            foreach (IModuleGraphSnapshotObserver observer in GetComponents<IModuleGraphSnapshotObserver>())
            {
                GraphEditor.AddObserver(observer);
            }

            IModule coreModule = Instantiate(_coreModulePrefab).GetComponent<IModule>();
            ICommandModuleGraph f = GraphEditor.StartEditing();
            f.AddModule(coreModule);
            GraphEditor.TrySaveChanges();
        }
    }
}