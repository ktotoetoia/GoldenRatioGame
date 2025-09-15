namespace IM.Economy
{
    public interface ICooldown : ICooldownReadOnly
    {
        void ForceReset();
        bool TryReset();
    }
}