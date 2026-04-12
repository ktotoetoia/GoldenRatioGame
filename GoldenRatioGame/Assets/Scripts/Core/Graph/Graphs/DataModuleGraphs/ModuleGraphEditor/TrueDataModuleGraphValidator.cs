namespace IM.Graphs
{
    public class TrueDataModuleGraphValidator<T> : IDataModuleGraphValidator<T>
    {
        public bool IsValid(IDataModuleGraph<T> obj) => true;
        public bool TryFix(IDataModuleGraph<T> graph) => true;
    }
}