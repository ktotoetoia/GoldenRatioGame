using IM.Graphs;
using IM.Items;

namespace IM.Modules
{
    public class AllowConnectionIfTagsMatch : IModuleGraphConditions
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
            if (output is IHaveTag outTg && input is IHaveTag inpTg)
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