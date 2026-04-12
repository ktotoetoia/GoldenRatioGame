using IM.LifeCycle;

namespace IM.Graphs
{
    public class AccessConditionalCommandDataModuleGraphFactory<T> : IFactory<AccessConditionalCommandDataModuleGraph<T>,
        IConditionalCommandDataModuleGraph<T>>
    {
        public AccessConditionalCommandDataModuleGraph<T> Create(IConditionalCommandDataModuleGraph<T> param1)
        {
            return new AccessConditionalCommandDataModuleGraph<T>(param1) {CanUse = true, ThrowIfCantUse = true};
        }
    }
}