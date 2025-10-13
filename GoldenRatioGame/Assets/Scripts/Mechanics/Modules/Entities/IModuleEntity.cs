using IM.Abilities;
using IM.Entities;
using IM.Graphs;

namespace IM.Modules
{
    public interface IModuleEntity : IEntity
    {
        IModuleGraphEditor<IConditionalCommandModuleGraph> GraphEditor { get; }
        IAbilityPool AbilityPool { get; }
    }
}