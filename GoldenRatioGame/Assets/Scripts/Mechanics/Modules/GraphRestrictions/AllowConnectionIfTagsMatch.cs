using IM.Graphs;
using IM.Items;

namespace IM.Modules
{
    public class AllowConnectionIfTagsMatch : IModuleGraphConditions
    {
        public bool CanConnect(IPort output, IPort input)
        {
            if (output is IHaveTag outTg && input is IHaveTag inpTg)
            {
                return outTg.Tag.Matches(inpTg.Tag) && inpTg.Tag.Matches(outTg.Tag);
            }

            return true;
        }
        public bool CanAddAndConnect(IModule module, IPort ownerPort, IPort targetPort)
        {
            return CanConnect(ownerPort, targetPort);
        }
    }
}