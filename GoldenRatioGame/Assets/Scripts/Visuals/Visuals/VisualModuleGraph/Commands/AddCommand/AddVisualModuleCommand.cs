using System;
using System.Collections.Generic;
using IM.Commands;
using IM.Graphs;

namespace IM.Visuals
{
    public class AddVisualModuleCommand : ICommand
    {
        private readonly ICollection<IModule> _addTo;
        private readonly IVisualModule _module;
        private readonly ITransform _parentTransform;
        private bool _isExecuted;

        public AddVisualModuleCommand(IVisualModule module, ICollection<IModule> addTo, ITransform parentTransform)
        {
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _addTo = addTo ?? throw new ArgumentNullException(nameof(addTo));
            _parentTransform = parentTransform ?? throw new ArgumentNullException(nameof(parentTransform));
        }

        public void Execute()
        {
            if (_isExecuted) throw new InvalidOperationException("Command already executed");
            if (_addTo.Contains(_module))
                throw new InvalidOperationException($"Other command already added this module {_module}");
            if (_parentTransform.ContainsChild(_module.Transform))
                throw new InvalidOperationException($"other command or user added the visual module ({_module}) to the transform ({_parentTransform})");
            
            _addTo.Add(_module);
            _parentTransform.AddChild(_module.Transform);
            _module.Transform.LocalPosition = _module.Transform.Position;
            _isExecuted = true;
        }

        public void Undo()
        {
            if (!_isExecuted) throw new InvalidOperationException("Command must be executed before undo");
            if (!_addTo.Contains(_module))
                throw new InvalidOperationException($"Other command already removed this module{_module}");
            if (!_parentTransform.ContainsChild(_module.Transform))
                throw new InvalidOperationException($"other command or user removed the visual module ({_module}) from the transform ({_parentTransform})");

            _addTo.Remove(_module);
            _parentTransform.RemoveChild(_module.Transform);
            _isExecuted = false;
        }
    }
}