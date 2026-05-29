namespace IM.EntityIntelligence
{
    public class EntityAction : IEntityAction
    {
        public virtual void OnEnter() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void OnExit() { }
    }
}