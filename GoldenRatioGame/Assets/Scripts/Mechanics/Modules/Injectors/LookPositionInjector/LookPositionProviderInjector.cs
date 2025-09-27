using IM.Graphs;

namespace IM.Modules
{
    public class LookPositionProviderInjector : IModuleObserver
    {
        private ILookPositionProvider _lookPositionProvider;

        public LookPositionProviderInjector() : this(new LegacyInputLookPositionProvider())
        {
            
        }
        
        public LookPositionProviderInjector(ILookPositionProvider lookPositionProvider)
        {
            _lookPositionProvider = lookPositionProvider;
        }
        
        public void Add(IModule module)
        {
            if (module is IRequireLookPosition l)
            {
                l.LookPositionProvider = _lookPositionProvider;
            }
        }

        public void Remove(IModule module)
        {
            if (module is IRequireLookPosition l)
            {
                l.LookPositionProvider = null;
            }
        }
    }
}