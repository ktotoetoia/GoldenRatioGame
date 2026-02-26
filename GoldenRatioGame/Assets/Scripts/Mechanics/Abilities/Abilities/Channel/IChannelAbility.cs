namespace IM.Abilities
{
    public interface IChannelAbility : IChannelAbilityReadOnly
    {
        bool TryChannel(out IChannelInfo channelInfo);
        void Interrupt();
    }
}