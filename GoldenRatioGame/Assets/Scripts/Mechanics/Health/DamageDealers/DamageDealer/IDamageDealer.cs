namespace IM.Health
{
    public interface IDamageDealer
    {
        /// <summary>
        /// Calculates how much damage this dealer would inflict
        /// on the given target without actually applying it.
        /// </summary>
        HealthChangeResult PreviewDamage(IDamageable target);
        /// <summary>
        /// Applies damage to the given target.
        /// </summary>
        HealthChangeResult DealDamage(IDamageable target);
    }
}