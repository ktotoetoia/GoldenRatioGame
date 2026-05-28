namespace IM.Effects
{
    public interface IBlockUserMovementEffectModifier : IEffectModifier
    {
        bool BlockUserMovement { get; }
    }
}