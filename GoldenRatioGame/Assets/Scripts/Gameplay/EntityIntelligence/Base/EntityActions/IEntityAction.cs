namespace IM.EntityIntelligence
{
    public interface IEntityAction
    {
        void OnEnter();
        void Update();
        void FixedUpdate();
        void OnExit();
    }
}