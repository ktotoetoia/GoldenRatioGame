namespace IM.Graphs
{
    public interface IConditionalCommandDataModuleGraph<T> : ICommandDataModuleGraph<T>, IDataModuleGraphConditions<T>
    {
        
    }
}