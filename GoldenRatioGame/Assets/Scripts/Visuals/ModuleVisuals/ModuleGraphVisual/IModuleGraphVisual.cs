using IM.Graphs;

namespace IM.Modules
{
    public interface IModuleGraphVisual
    {
        IModuleGraphReadOnly Source { get;}
        
        void SetSource(IModuleGraphReadOnly source, ICoreGameModule coreModule);
    }
}