using IM.Base;

namespace IM.Graphs
{
    public class
        AccessConditionalCommandModuleGraphFactory : IFactory<AccessConditionalCommandModuleGraph,
        IConditionalCommandModuleGraph>
    {
        public AccessConditionalCommandModuleGraph Create(IConditionalCommandModuleGraph param1)
        {
            return new AccessConditionalCommandModuleGraph(param1) {CanUse = true, ThrowIfCantUse = true};
        }
    }
}