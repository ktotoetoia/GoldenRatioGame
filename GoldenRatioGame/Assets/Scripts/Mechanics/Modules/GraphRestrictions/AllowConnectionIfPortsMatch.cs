using System.Linq;
using IM.Graphs;

namespace IM.Modules
{
    public class AllowConnectionIfPortsMatch : IModuleGraphConditions
    {
        public bool CanConnect(IPort output, IPort input)
        {
            if (output is IConditionalPort outputC && input is IConditionalPort inputC)
            {
                return outputC.CanConnect(input) && inputC.CanConnect(output);
            }
            
            return true;
        }
        public bool CanAddAndConnect(IModule module, IPort ownerPort, IPort targetPort)
        {
            return CanConnect(ownerPort, targetPort);
        }

        public bool CanDisconnect(IConnection connection)
        {
            if (connection.Port1 is not IConditionalPort p1 || connection.Port2 is not IConditionalPort p2)
                return true;

            return p1.CanDisconnect() && p2.CanDisconnect();
        }

        public bool CanRemoveModule(IModule module)
        {
            return module.Ports.All(port => !port.IsConnected || CanDisconnect(port.Connection));
        }
    }
}