using System;
using IM.Graphs;
using IM.Storages;

namespace IM.Modules
{
    public class ModuleTransitioner
    {
        public void ClearState(IExtensibleModule module, IItemMutableStorage storage, IModuleGraph graph)
        {
            if(module.State == ModuleState.StorableState) storage?.ClearCell(module.Cell);
            if(module.State == ModuleState.GraphState) graph?.RemoveModule(module);
            
            module.State = ModuleState.None;
        }
        
        public void SetStateToGround(IExtensibleModule module)
        {
            if(module.State == ModuleState.GroundState) throw new InvalidOperationException("Module already in the ground state");
            
            module.State = ModuleState.GroundState;
        }
        
        public void SetStateToGraphAndAdd(IExtensibleModule module,IPort ownerPort, IPort targetPort, IModuleGraph graph)
        {
            if(module.State == ModuleState.GraphState) throw new InvalidOperationException("Module already in the graph state");
            
            graph.AddAndConnect(module, ownerPort, targetPort);
            module.State = ModuleState.GraphState;
        }

        public void SetStateToStorage(IExtensibleModule module, IItemMutableStorage storage, IStorageCell cell)
        {
            if(module.State ==  ModuleState.StorableState) throw new InvalidOperationException("Module already in the storable state");
            
            storage.SetItem(cell, module);
            module.State = ModuleState.StorableState;
        }
    }
}