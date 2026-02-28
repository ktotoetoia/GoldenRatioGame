namespace IM.Base
{
    public interface IPoolObject
    {
        void OnRelease();
        void OnGet();
    }
}