using IM.Graphs;

namespace IM.Modules
{
    public class PortTagsModuleGraphConditions : IModuleGraphConditions
    {
        public bool CanAddModule(IModule module)
        {
            return true;
        }

        public bool CanRemoveModule(IModule module)
        {
            return true;
        }

        public bool CanConnect(IPort output, IPort input)
        {
            if (input == output || output.Module == input.Module || output.IsConnected || input.IsConnected)
            {
                return false;
            }
            
            if (output is TaggedPort outTg && input is TaggedPort inpTg)
            {
                return outTg.Tag.Equals(inpTg.Tag);
            }

            return true;
        }

        public bool CanDisconnect(IConnection connection)
        {
            return true;
        }

        public bool CanAddAndConnect(IModule module, IPort ownerPort, IPort targetPort)
        {
            return CanAddModule(module) && CanConnect(ownerPort, targetPort);
        }
    }
}