using System;
using System.Collections.Generic;
using IM.Commands;

namespace IM.Graphs
{
    public class AddModuleCommand : Command
    {
        private readonly ICollection<IModule> _addTo;
        private readonly IModule _module;

        public AddModuleCommand(IModule module, ICollection<IModule> addTo)
        {
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _addTo = addTo ?? throw new ArgumentNullException(nameof(addTo));
        }

        protected override void InternalExecute()
        {
            if (_addTo.Contains(_module))
                throw new InvalidOperationException($"Other command already added this module {_module}");

            _addTo.Add(_module);
        }

        protected override void InternalUndo()
        {
            if (!_addTo.Contains(_module))
                throw new InvalidOperationException($"Other command already removed this module{_module}");

            _addTo.Remove(_module);
        }
    }
}