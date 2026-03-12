namespace IM.Abilities
{
    public interface IChannelAbilityReadOnly : IAbilityReadOnly
    {
        bool IsChanneling { get; }
    }
}