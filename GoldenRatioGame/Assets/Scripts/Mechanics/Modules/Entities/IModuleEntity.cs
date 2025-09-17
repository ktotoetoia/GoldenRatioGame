using IM.Abilities;
using IM.Entities;
using IM.Graphs;

namespace IM.Modules
{
    public interface IModuleEntity : IEntity
    {
        ICoreModuleGraph Graph { get; }
        IAbilitiesPool AbilitiesPool { get; }
    }
}