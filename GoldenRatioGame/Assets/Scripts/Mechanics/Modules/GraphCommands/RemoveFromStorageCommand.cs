using System;
using IM.Commands;
using IM.Storages;

namespace IM.Modules
{
    public class RemoveFromStorageCommand : Command
    {
        private readonly ICommand _command;
        private readonly IStorage _storage;
        private readonly IStorableReadOnly _storable;
        private readonly IStorageCellReadonly _cell;
        
        public RemoveFromStorageCommand(ICommand command, IStorageCellReadonly cell, IStorableReadOnly storable)
        {
            _command = command ?? throw new ArgumentNullException(nameof(command));
            _cell = cell ?? throw new ArgumentNullException(nameof(cell));
            _storage = cell.Owner ?? throw new InvalidOperationException("Cell has no owner.");
            _storable = storable  ?? throw new ArgumentNullException(nameof(storable));
        }

        protected override void InternalExecute()
        {
            if (_cell.Item != _storable) throw new InvalidOperationException("Cell was modified by another operation");
            
            _command.Execute();
            
            _storage.ClearCell(_cell);
        }

        protected override void InternalUndo()
        {
            if (_cell.Item != null) throw new InvalidOperationException("Cell was modified by another operation");
            
            _command.Undo();
            
            _storage.SetItem(_cell,_storable);
        }
    }
}