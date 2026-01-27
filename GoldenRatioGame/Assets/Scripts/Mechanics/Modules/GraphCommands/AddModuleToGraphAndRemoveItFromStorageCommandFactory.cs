using System;
using System.Collections.Generic;
using IM.Commands;
using IM.Graphs;
using IM.Storages;

namespace IM.Modules
{
    public class AddModuleToGraphAndRemoveItFromStorageCommandFactory : IAddModuleCommandFactory
    {
        public ICommand Create(IModule param1, ICollection<IModule> param2)
        {
            IStorableReadOnly storable = param1 as IStorableReadOnly ?? throw new ArgumentException(); 
            
            return new RemoveFromStorageCommand(new AddModuleCommand(param1, param2),storable.Cell,storable);
        }
    }
}