namespace IM.LifeCycle
{
    public interface IIdentifiable
    {
        string Id { get; }
        void InjectId(string id);
    }
}