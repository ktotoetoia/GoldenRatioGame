using System;
using System.Collections.Generic;
using IM.Commands;

namespace IM.Graphs
{
    public class AddModuleCommand : ICommand
    {
        private readonly ICollection<IModule> _addTo;
        private readonly IModule _module;
        private bool _isExecuted;

        public AddModuleCommand(IModule module, ICollection<IModule> addTo)
        {
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _addTo = addTo ?? throw new ArgumentNullException(nameof(addTo));
        }

        public void Execute()
        {
            if (_isExecuted) throw new InvalidOperationException("Command already executed");
            if (_addTo.Contains(_module))
                throw new InvalidOperationException($"Other command already added this module {_module}");

            _addTo.Add(_module);
            _isExecuted = true;
        }

        public void Undo()
        {
            if (!_isExecuted) throw new InvalidOperationException("Command must be executed before undo");
            if (!_addTo.Contains(_module))
                throw new InvalidOperationException($"Other command already removed this module{_module}");

            _addTo.Remove(_module);
            _isExecuted = false;
        }
    }
}