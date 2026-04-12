using System.Collections.Generic;
using IM.Commands;

namespace IM.Graphs
{
    public class RemoveAndDisconnectDataModulesCommandFactory<T> : IRemoveDataModuleCommandFactory<T>
    {
        public ICommand Create(IDataModule<T> param1, ICollection<IDataModule<T>> param2, ICollection<IDataConnection<T>> param3)
        {
            return new RemoveAndDisconnectDataModulesCommand<T>(param1, param2, param3);
        }
    }
}