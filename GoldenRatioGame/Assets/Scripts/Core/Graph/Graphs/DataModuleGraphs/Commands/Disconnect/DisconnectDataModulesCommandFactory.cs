using System.Collections.Generic;
using IM.Commands;

namespace IM.Graphs
{
    public class DisconnectDataModulesCommandFactory<T> : IDisconnectDataCommandFactory<T>
    {
        public ICommand Create(IDataConnection<T> param1, ICollection<IDataConnection<T>> param2)
        {
            return new DisconnectDataModulesCommand<T>(param1, param2);
        }
    }
}