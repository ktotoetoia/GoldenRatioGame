namespace IM.SaveSystem
{
    public interface IIdentifiable
    {
        string Id { get; }
        void InjectId(string id);
    }
}