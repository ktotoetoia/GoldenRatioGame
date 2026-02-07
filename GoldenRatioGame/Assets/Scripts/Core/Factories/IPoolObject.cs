namespace IM.Visuals
{
    public interface IPoolObject
    {
        void OnRelease();
        void OnGet();
    }
}