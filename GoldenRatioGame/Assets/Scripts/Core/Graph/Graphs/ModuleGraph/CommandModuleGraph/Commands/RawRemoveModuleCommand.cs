using System;
using System.Collections.Generic;
using ICommand = IM.Commands.ICommand;

namespace IM.Graphs
{
    public class RawRemoveModuleCommand : ICommand
    {
        private readonly ICollection<IModule> _removeFrom;
        private readonly IModule _module;
        private bool _isExecuted;

        public RawRemoveModuleCommand(IModule module, ICollection<IModule> removeFrom)
        {
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _removeFrom = removeFrom ?? throw new ArgumentNullException(nameof(removeFrom));
        }

        public void Execute()
        {
            if (_isExecuted) throw new InvalidOperationException("Command already executed");
            if (!_removeFrom.Contains(_module))
                throw new InvalidOperationException(
                    $"This module does not exist in the collection or other command already removed it: {_module}");

            _removeFrom.Remove(_module);
            _isExecuted = true;
        }

        public void Undo()
        {
            if (!_isExecuted) throw new InvalidOperationException("Command must be executed before undo");
            if (_removeFrom.Contains(_module))
                throw new InvalidOperationException($"Other command added this module back{_module}");

            _removeFrom.Add(_module);
            _isExecuted = false;
        }
    }
}