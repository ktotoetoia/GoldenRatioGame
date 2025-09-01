namespace IM.Health
{
    public class RawFloatDamageDealer : IFloatDamageDealer
    {
        public float Damage { get; set; }

        public RawFloatDamageDealer(float damage)
        {
            Damage = damage;   
        }
        
        public DamageResult PreviewDamage(IDamageable target)
        {
            return target.PreviewDamage(Damage);
        }

        public DamageResult DealDamage(IDamageable target)
        {
            return target.ApplyDamage(Damage);
        }
    }
}