namespace IM.LifeCycle
{
    public interface IPoolObject
    {
        void OnRelease();
        void OnGet();
    }
}