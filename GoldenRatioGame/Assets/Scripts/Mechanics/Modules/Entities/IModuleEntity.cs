using IM.Entities;

namespace IM.Modules
{
    public interface IModuleEntity : IEntity
    {
        ICoreModuleGraph Graph { get; }
    }
}