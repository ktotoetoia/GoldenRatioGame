using System;
using System.Collections.Generic;
using IM.Commands;

namespace IM.Graphs
{
    public class RemoveModuleCommand : Command
    {
        private readonly ICollection<IModule> _removeFrom;
        private readonly IModule _module;

        public RemoveModuleCommand(IModule module, ICollection<IModule> removeFrom)
        {
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _removeFrom = removeFrom ?? throw new ArgumentNullException(nameof(removeFrom));
        }

        protected override void InternalExecute()
        {
            if (!_removeFrom.Contains(_module))
                throw new InvalidOperationException(
                    $"This module does not exist in the collection or other command already removed it: {_module}");

            _removeFrom.Remove(_module);
        }

        protected override void InternalUndo()
        {
            if (_removeFrom.Contains(_module))
                throw new InvalidOperationException($"Other command added this module back{_module}");

            _removeFrom.Add(_module);
        }
    }
}