namespace IM.Health
{
    public interface IDamageDealer
    {
        /// <summary>
        /// Calculates how much damage this dealer would inflict
        /// on the given target without actually applying it.
        /// </summary>
        DamageResult PreviewDamage(IDamageable target);
        /// <summary>
        /// Applies damage to the given target.
        /// </summary>
        DamageResult DealDamage(IDamageable target);
    }
}