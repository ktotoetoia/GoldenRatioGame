using System;
using System.Collections.Generic;
using IM.Commands;
using IM.Graphs;
using IM.Storages;

namespace IM.Modules
{
    public class RemoveModuleFromGraphAndAddItToStorageCommandFactory : IRemoveModuleCommandFactory
    {
        private readonly Func<IModule, IStorageCellReadonly> _getStorageCell;
        
        public RemoveModuleFromGraphAndAddItToStorageCommandFactory(Func<IModule, IStorageCellReadonly> getStorageCell)
        {
            _getStorageCell = getStorageCell;
        }

        public ICommand Create(IModule param1, ICollection<IModule> param2, ICollection<IConnection> param3)
        {
            IStorableReadOnly storable = param1 as IStorableReadOnly ?? throw new ArgumentException();
            return new AddToStorageCommand(new RemoveAndDisconnectModuleCommand(param1, param2,param3),_getStorageCell(param1),storable);
        }
    }
}