using IM.Graphs;

namespace IM.Entities
{
    public interface IModuleEntity : IEntity
    {
        IModuleGraph Graph { get; }
    }
}