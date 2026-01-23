using IM.Abilities;
using IM.Graphs;
using IM.Storages;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEntity : MonoBehaviour, IModuleEntity
    {
        public GameObject GameObject => gameObject;
        public IModuleGraphEditor<IConditionalCommandModuleGraph> GraphEditor { get; private set; }
        public IAbilityPool AbilityPool { get; private set; } = new AbilityPool();

        public void Initialize(IModule coreModule)
        {
            ConditionalCommandModuleGraph graph = new ConditionalCommandModuleGraph(new CommandModuleGraph(), new PortTagsModuleGraphConditions());
            GraphEditor = new CommandModuleGraphEditor<IConditionalCommandModuleGraph>(graph,new AccessConditionalCommandModuleGraphFactory());
            
            foreach (IModuleGraphSnapshotObserver observer in GetComponents<IModuleGraphSnapshotObserver>())
            {
                GraphEditor.AddObserver(observer);
            }

            ICommandModuleGraph f = GraphEditor.StartEditing();
            f.AddModule(coreModule);
            GraphEditor.TrySaveChanges();
        }
    }
}