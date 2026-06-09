using System.Linq;
using IM.LifeCycle;

namespace IM.Modules
{
    public class RequireNumberOfModulesGraphValidator<TContext> : IValidator<TContext> where TContext : IModuleEditingContext
    {
        public int Count { get; set; }

        public RequireNumberOfModulesGraphValidator(int count)
        {
            Count = count;
        }
        
        public bool IsValid(TContext obj)
        {
            if (obj.Storage.Count(x => x.Item!=null) == 0) return true;
            
            return obj.Graph.Modules.Count >= Count;
        }

        public bool TryFix(TContext obj) => IsValid(obj);
    }
}