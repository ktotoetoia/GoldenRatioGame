namespace IM.Health
{
    public interface IDamageable
    {
        /// <summary>
        /// Calculates how much damage would actually be applied
        /// if this damageable took the given amount.
        /// </summary>
        DamageResult PreviewDamage(float incomingDamage);
        /// <summary>
        /// Applies the given amount of damage.
        /// </summary>
        DamageResult ApplyDamage(float damage);
    }

    public interface IHealable
    {
        HealingResult PreviewHealing(float healing);
        HealingResult ApplyHealing(float healing);
    }

    public interface IHealth : IDamageable, IHealable
    {
        
    }
}