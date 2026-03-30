namespace IM.LifeCycle
{
    public interface IRequireGameObjectFactory
    {
        IGameObjectFactory Factory { get; set; }
    }
}