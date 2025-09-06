namespace IM.Health
{
    public class RawFloatDamageDealer : IFloatDamageDealer
    {
        public float Damage { get; set; }

        public RawFloatDamageDealer(float damage)
        {
            Damage = damage;   
        }
        
        public HealthChangeResult PreviewDamage(IDamageable target)
        {
            return target.PreviewDamage(Damage);
        }

        public HealthChangeResult DealDamage(IDamageable target)
        {
            return target.TakeDamage(Damage);
        }
    }
}