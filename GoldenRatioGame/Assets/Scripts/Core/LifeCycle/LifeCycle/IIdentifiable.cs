namespace IM.LifeCycle
{
    public interface IIdentifiable : IHaveID
    {
        void Inject(string id);
    }
}