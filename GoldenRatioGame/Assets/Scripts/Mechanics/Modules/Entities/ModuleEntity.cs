using IM.Abilities;
using IM.Graphs;
using IM.Health;
using IM.Values;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEntity : MonoBehaviour, IModuleEntity
    {
        [SerializeField] private GameObject _coreModulePrefab;

        public GameObject GameObject => gameObject;
        public IModuleGraphEditor<IConditionalCommandModuleGraph> GraphEditor { get; private set; }
        public IAbilityPool AbilityPool { get; private set; }

        private void Awake()
        {
            IModule coreModule = Instantiate(_coreModulePrefab).GetComponent<IModule>();
            ConditionalCommandModuleGraph graph = new ConditionalCommandModuleGraph(new CommandModuleGraph(), new PortTagsModuleGraphConditions());
            AbilityPool = new AbilityPool();
            GraphEditor = new CommandModuleGraphEditor<IConditionalCommandModuleGraph>(graph,new AccessConditionalCommandModuleGraphFactory());
            
            GraphEditor.AddObserver(new EntityInjector(this));
            GraphEditor.AddObserver(new SpeedExtensionsObserver(GetComponent<IHaveSpeed>()));
            GraphEditor.AddObserver(new HealthExtensionsObserver(GetComponent<IFloatHealthValuesGroup>()));
            GraphEditor.AddObserver(new AbilityExtensionsObserver(AbilityPool as AbilityPool));
            
            foreach (IModuleGraphObserver observer in GetComponents<IModuleGraphObserver>())
            {
                GraphEditor.AddObserver(observer);
            }

            ICommandModuleGraph f = GraphEditor.StartEditing();
            f.AddModule(coreModule);
            GraphEditor.TrySaveChanges();
        }
    }
}