namespace IM.Health
{
    public readonly struct HealingResult
    {
        public float HealthBefore { get; }
        public float HealthAfter { get; }

        public float PreMitigationHealing { get; }
        public float HealingMitigated { get; }
        public float PostMitigationHealing { get; }
        public float Overflow { get; }
        public bool IsFullHeal => HealthAfter >= HealthBefore + PreMitigationHealing;

        public HealingResult(float healthBefore, float healthAfter, float preMitigationHealing, float healingMitigated)
        {
            HealthBefore = healthBefore;
            HealthAfter = healthAfter;
            PreMitigationHealing = preMitigationHealing;
            HealingMitigated = healingMitigated;

            float effective = preMitigationHealing - healingMitigated;
            float applied = healthAfter - healthBefore;

            PostMitigationHealing = applied;
            Overflow = effective - applied;
        }
    }
}