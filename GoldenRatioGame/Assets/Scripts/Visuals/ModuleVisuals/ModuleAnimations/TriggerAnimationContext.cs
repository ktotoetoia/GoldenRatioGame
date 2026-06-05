namespace IM.Visuals
{
    public class TriggerAnimationContext : IAnimationContext
    {
        public string Name { get; }
        public float Duration { get; }

        public TriggerAnimationContext(string name, float duration)
        {
            Name = name;
            Duration = duration;
        }
    }
}