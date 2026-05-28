namespace IM.Effects
{
    public interface ITemporaryEffectGroup : IEffectGroup
    {
        float EstimatedDuration { get; }
        float EstimatedTimeLeft { get; }
        bool IsFinished { get; }
    }
}