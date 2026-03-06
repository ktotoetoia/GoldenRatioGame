namespace IM.Common
{
    public interface ICooldown : ICooldownReadOnly
    {
        void ForceReset();
        bool TryReset();
    }
}