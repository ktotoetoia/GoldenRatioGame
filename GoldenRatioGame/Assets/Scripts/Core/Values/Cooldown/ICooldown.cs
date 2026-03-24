namespace IM.Values
{
    public interface ICooldown : ICooldownReadOnly
    {
        void ForceReset();
        bool TryReset();
    }
}