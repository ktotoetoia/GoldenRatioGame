using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class AllowConnectionIfPortsMatch<T> :  IDataModuleGraphConditions<T>
    {
        public bool CanConnect(IDataPort<T> output, IDataPort<T> input)
        {
            if (output is IConditionalPort outputC && input is IConditionalPort inputC)
            {
                return outputC.CanConnect(input) && inputC.CanConnect(output);
            }
            
            return true;
        }
        
        public bool CanAddAndConnect(IDataModule<T> module, IDataPort<T> ownerPort, IDataPort<T> targetPort)
        {
            return CanConnect(ownerPort, targetPort);
        }

        public bool CanDisconnect(IDataConnection<T> connection)
        {
            if (connection.Port1 is not IConditionalPort p1 || connection.Port2 is not IConditionalPort p2)
                return true;

            return p1.CanDisconnect() && p2.CanDisconnect();
        }

        public bool CanRemove(IDataModule<T> module)
        {
            return module.DataPorts.All(port => !port.IsConnected || CanDisconnect(port.DataConnection));
        }
    }
}