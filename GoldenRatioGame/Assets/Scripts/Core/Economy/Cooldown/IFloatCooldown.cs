namespace IM.Economy
{
    public interface IFloatCooldown : ICooldown, ICappedValueReadOnly<float>
    {
        float TotalCooldown { get; }
        float RemainingCooldown { get; }
    }
}