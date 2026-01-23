using System.Collections.Generic;
using IM.Abilities;
using IM.Graphs;
using NUnit.Framework;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEntity : MonoBehaviour, IModuleEntity
    {
        public GameObject GameObject => gameObject;
        public IModuleGraphEditor<IConditionalCommandModuleGraph> GraphEditor { get; private set; }
        public IAbilityPool AbilityPool { get; } = new AbilityPool();

        public void Initialize(IExtensibleModule coreModule)
        {
            CommandModuleGraph graph = new();
            
            ConditionalCommandModuleGraph conditionalCommandModuleGraph = new ConditionalCommandModuleGraph(graph, 
                new CompositeModuleGraphConditions(new List<IModuleGraphConditions>()
            {
                new DefaultModuleGraphConditions(graph),
                new PortTagsModuleGraphConditions(),
            }));
            
            GraphEditor = new CommandModuleGraphEditor<IConditionalCommandModuleGraph>(conditionalCommandModuleGraph,new AccessConditionalCommandModuleGraphFactory());
            
            foreach (IModuleGraphSnapshotObserver observer in GetComponents<IModuleGraphSnapshotObserver>())
            {
                GraphEditor.AddObserver(observer);
            }

            ICommandModuleGraph f = GraphEditor.StartEditing();
            coreModule.State = ModuleState.GraphState;
            f.AddModule(coreModule);
            GraphEditor.TrySaveChanges();
        }
    }
}