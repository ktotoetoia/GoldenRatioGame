namespace IM.Health
{
    public readonly struct DamageResult
    {
        public float HealthBefore { get; }
        public float HealthAfter { get; }
        public float PreMitigationDamage { get; }
        public float DamageMitigated { get; }
        public float PostMitigationDamage { get; }
        public float Overflow { get; }
        
        public DamageResult(float healthBefore, float healthAfter, float preMitigationDamage, float damageMitigated)
        {
            HealthBefore = healthBefore;
            HealthAfter = healthAfter;
            PreMitigationDamage = preMitigationDamage;
            DamageMitigated = damageMitigated;

            float effective = preMitigationDamage - damageMitigated;
            float applied = healthBefore - healthAfter;

            PostMitigationDamage = applied;
            Overflow = effective - applied;
        }
    }
}