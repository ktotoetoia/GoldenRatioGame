namespace IM.Graphs
{
    public class TrueModuleGraphValidator : IModuleGraphValidator
    {
        public bool IsValid(IModuleGraphReadOnly graph)
        {
            return true;
        }

        public bool TryFix(IModuleGraph graph)
        {
            return true;
        }
    }
}