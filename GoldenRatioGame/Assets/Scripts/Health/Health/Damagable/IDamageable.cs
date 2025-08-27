namespace IM.Health
{
    public interface IDamageable
    {
        /// <summary>
        /// Calculates how much damage would actually be applied
        /// if this damageable took the given amount.
        /// </summary>
        float PreviewDamage(float incomingDamage);
        /// <summary>
        /// Applies the given amount of damage.
        /// </summary>
        void ApplyDamage(float damage);
    }
}