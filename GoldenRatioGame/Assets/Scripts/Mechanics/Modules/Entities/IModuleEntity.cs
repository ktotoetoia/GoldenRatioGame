using IM.Abilities;
using IM.Entities;
using IM.Graphs;

namespace IM.Modules
{
    public interface IModuleEntity : IEntity
    {
        IObservableModuleGraph Graph { get; }
        IAbilityPool AbilityPool { get; }
    }
}