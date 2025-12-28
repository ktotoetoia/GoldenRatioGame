namespace TDS.Handlers
{
    public interface IHandler<in T>
    {
        void Handle(T operation);
    }
}