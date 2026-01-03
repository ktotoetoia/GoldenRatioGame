using System;
using System.Collections.Generic;
using IM.Commands;
using IM.Graphs;

namespace IM.Visuals
{
    public class AddTransformModuleCommand : Command
    {
        private readonly ICollection<IModule> _addTo;
        private readonly ITransformModule _module;
        private readonly IHierarchyTransform _parentTransform;

        public AddTransformModuleCommand(ITransformModule module, ICollection<IModule> addTo, IHierarchyTransform parentTransform)
        {
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _addTo = addTo ?? throw new ArgumentNullException(nameof(addTo));
            _parentTransform = parentTransform ?? throw new ArgumentNullException(nameof(parentTransform));
        }

        protected override void InternalExecute()
        {
            if (_addTo.Contains(_module))
                throw new InvalidOperationException($"Other command already added this module {_module}");
            if (_parentTransform.ContainsChild(_module.HierarchyTransform))
                throw new InvalidOperationException($"other command or user added the visual module ({_module}) to the transform ({_parentTransform})");
            
            _addTo.Add(_module);
            _parentTransform.AddChild(_module.HierarchyTransform);
            _module.HierarchyTransform.LocalPosition = _module.HierarchyTransform.Position;
        }

        protected override void InternalUndo()
        {
            if (!_addTo.Contains(_module))
                throw new InvalidOperationException($"Other command already removed this module {_module}");
            if (!_parentTransform.ContainsChild(_module.HierarchyTransform))
                throw new InvalidOperationException($"other command or user removed the visual module ({_module}) from the transform ({_parentTransform})");

            _addTo.Remove(_module);
            _parentTransform.RemoveChild(_module.HierarchyTransform);
        }
    }
}