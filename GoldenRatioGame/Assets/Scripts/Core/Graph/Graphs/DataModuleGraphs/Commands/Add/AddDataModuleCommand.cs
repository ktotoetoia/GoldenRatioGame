using System;
using System.Collections.Generic;
using IM.Commands;

namespace IM.Graphs
{
    public class AddDataModuleCommand<T> : Command
    {
        private readonly ICollection<IDataModule<T>> _addTo;
        private readonly IDataModule<T> _module;
        
        public AddDataModuleCommand(IDataModule<T> module, ICollection<IDataModule<T>> addTo)
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