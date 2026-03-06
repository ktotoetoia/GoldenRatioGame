namespace IM.Common
{
    public interface IPoolObject
    {
        void OnRelease();
        void OnGet();
    }
}