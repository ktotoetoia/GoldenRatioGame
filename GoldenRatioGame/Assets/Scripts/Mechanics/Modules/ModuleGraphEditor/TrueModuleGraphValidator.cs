using IM.Graphs;

namespace IM.Modules
{
    public class TrueModuleGraphValidator : IModuleGraphValidator
    {
        public bool IsValid(IModuleGraphReadOnly graph)
        {
            return true;
        }
    }
}