namespace IM.Effects
{
    public interface IBlockUserMovementEffect : IEffectModifier
    {
        bool BlockUserMovement { get; }
    }
}