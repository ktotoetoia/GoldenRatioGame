namespace IM.LifeCycle
{
    public interface IValidator<in T>
    {
        bool IsValid(T obj);
        bool TryFix(T obj);
    }
}