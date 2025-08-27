namespace IM.Health
{
    public class RawFloatDamageDealer : IFloatDamageDealer
    {
        public float Damage { get; set; }

        public RawFloatDamageDealer(float damage)
        {
            Damage = damage;   
        }
        
        public float PreviewDamage(IDamageable target)
        {
            return target.PreviewDamage(Damage);
        }

        public void DealDamage(IDamageable target)
        {
            target.ApplyDamage(Damage);
        }
    }
}