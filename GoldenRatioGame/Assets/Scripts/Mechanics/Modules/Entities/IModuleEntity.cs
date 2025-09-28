using IM.Abilities;
using IM.Entities;
using IM.Graphs;

namespace IM.Modules
{
    public interface IModuleEntity : IEntity
    {
        IGameModuleGraph Graph { get; }
        IAbilityPool AbilityPool { get; }
    }
}