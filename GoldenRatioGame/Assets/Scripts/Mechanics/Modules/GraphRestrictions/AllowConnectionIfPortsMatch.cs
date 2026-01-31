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
            return connection.Port1 is not IConditionalPort port1 || connection.Port2 is not IConditionalPort port2|| 
                   (port1.CanConnect(port2) && port2.CanConnect(port1));
        }

        public bool CanRemoveModule(IModule module)
        {
            return module.Ports.Where(x => x.IsConnected).All(x => CanDisconnect(x.Connection));
        }
    }
}