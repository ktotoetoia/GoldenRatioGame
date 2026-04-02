using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.LifeCycle;
using IM.SaveSystem;
using UnityEngine;

namespace IM.Modules
{
    public class AbilityModuleEditingContextSerializer : ComponentSerializer<AbilityModuleEditingContextMono>
    {
        public override object CaptureState(AbilityModuleEditingContextMono component)
        {
            ModuleEditingContextState state = new ModuleEditingContextState();

            state.StorageItemsIds.AddRange(component.Storage
                .Select(cell => cell.Item as IExtensibleModule)
                .Select(TryGetModuleId)
                .Where(id => id != null));

            HashSet<(string moduleId, int portId)> processedPorts = new HashSet<(string moduleId, int portId)>();

            foreach (IExtensibleModule module in component.GraphEditor.Snapshot.Modules.OfType<IExtensibleModule>())
            {
                string currentId = TryGetModuleId(module);
                if (currentId == null) continue;

                foreach (IPort port in module.Ports.Where(p => p.IsConnected))
                {
                    int currentPortId = module.GetPortId(port);

                    if (!processedPorts.Add((currentId, currentPortId))) continue; 

                    IPort targetPort = port.Connection.GetOtherPort(port); 
                    if (targetPort?.Module is not IExtensibleModule targetModule) continue;

                    string targetId = TryGetModuleId(targetModule);
                    if (targetId == null) continue;

                    int targetPortId = targetModule.GetPortId(targetPort);

                    state.Links.Add(new GraphLinkData
                    {
                        ModuleA_Id = currentId,
                        PortA_Id = currentPortId,
                        ModuleB_Id = targetId,
                        PortB_Id = targetPortId
                    });

                    processedPorts.Add((targetId, targetPortId));
                }
            }

            return state;
        }

        public override void RestoreState(AbilityModuleEditingContextMono component, object state, Func<string, GameObject> resolveDependency)
        {
            if (component.GraphEditor.IsEditing) 
                throw new InvalidOperationException("Cannot restore state while the graph editor is currently in edit mode.");

            ModuleEditingContextState savedState = (ModuleEditingContextState)state;

            HashSet<string> requiredModuleIds = new HashSet<string>(savedState.StorageItemsIds);
            foreach (GraphLinkData link in savedState.Links)
            {
                requiredModuleIds.Add(link.ModuleA_Id);
                requiredModuleIds.Add(link.ModuleB_Id);
            }

            foreach (string id in requiredModuleIds)
            {
                GameObject go = resolveDependency(id);
                if (go != null && go.TryGetComponent(out IExtensibleModule module) && !component.Storage.ContainsItem(module))
                {
                    component.AddToContext(module);
                }
            }

            component.SetUnsafe(true);
            try
            {
                IModuleGraph graph = component.GraphEditor.BeginEdit();
                RebuildGraphLinks(graph, savedState.Links, resolveDependency);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                if (!component.GraphEditor.TryApplyChanges())
                {
                    component.GraphEditor.DiscardChanges();
                }
                component.SetUnsafe(false);
            }
        }

        private void RebuildGraphLinks(IModuleGraph graph, IEnumerable<GraphLinkData> links, Func<string, GameObject> resolveDependency)
        {
            foreach (GraphLinkData link in links)
            {
                GameObject goA = resolveDependency(link.ModuleA_Id);
                GameObject goB = resolveDependency(link.ModuleB_Id);

                if (goA == null || goB == null) continue;

                if (goA.TryGetComponent(out IExtensibleModule modA) && goB.TryGetComponent(out IExtensibleModule modB))
                {
                    if (!graph.Contains(modA)) graph.AddModule(modA);
                    if (!graph.Contains(modB)) graph.AddModule(modB);
                    
                    IPort portA = modA.GetPort(link.PortA_Id);
                    IPort portB = modB.GetPort(link.PortB_Id);

                    if (portA != null && portB != null)
                    {
                        graph.Connect(portA, portB);
                    }
                }
            }
        }

        private string TryGetModuleId(IExtensibleModule module)
        {
            return module?.Extensions.TryGet(out IIdentifiable identifiable) == true ? identifiable.Id : null;
        }

        [Serializable]
        public class GraphLinkData
        {
            public string ModuleA_Id;
            public int PortA_Id;
            public string ModuleB_Id;
            public int PortB_Id;
        }

        [Serializable]
        public class ModuleEditingContextState
        {
            public List<string> StorageItemsIds = new();
            public List<GraphLinkData> Links = new();
        }
    }
}