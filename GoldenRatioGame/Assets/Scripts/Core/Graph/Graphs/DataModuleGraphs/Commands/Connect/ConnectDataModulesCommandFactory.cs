using System.Collections.Generic;

namespace IM.Graphs
{
    public class ConnectDataModulesCommandFactory<T> : IConnectDataModulesCommandFactory<T>
    {
        public IDataConnectCommand<T> Create(IDataPort<T> param1, IDataPort<T> param2, ICollection<IDataConnection<T>> param3)
        {
            return new ConnectDataModuleCommand<T>(param1, param2, param3);
        }
    }
}