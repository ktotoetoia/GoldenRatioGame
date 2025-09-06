namespace IM.Health
{
    public interface IHealable
    {
        HealthChangeResult PreviewHealing(float healing);
        HealthChangeResult RestoreHealth(float healing);
    }
}