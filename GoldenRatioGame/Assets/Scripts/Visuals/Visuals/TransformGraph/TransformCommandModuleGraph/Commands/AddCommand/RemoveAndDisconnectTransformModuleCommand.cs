using System;
using System.Collections.Generic;
using IM.Commands;
using IM.Graphs;

namespace IM.Visuals
{
    public class RemoveAndDisconnectTransformModuleCommand : Command
    {
        private readonly RemoveAndDisconnectModuleCommand _removeAndDisconnectModuleCommand;
        private readonly ITransformModule _module;
        private readonly IHierarchyTransform _parent;
        
        public RemoveAndDisconnectTransformModuleCommand(ITransformModule module, ICollection<IModule> modules, ICollection<IConnection> connections, IHierarchyTransform parent)
        {
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _removeAndDisconnectModuleCommand = new RemoveAndDisconnectModuleCommand(module, modules, connections);
            _parent = parent;
        }

        protected override void InternalExecute()
        {
            _removeAndDisconnectModuleCommand.Execute();
            _parent.RemoveChild(_module.HierarchyTransform);
        }

        protected override void InternalUndo()
        {
            _removeAndDisconnectModuleCommand.Undo();
            _parent.AddChild(_module.HierarchyTransform);
        }
    }
}