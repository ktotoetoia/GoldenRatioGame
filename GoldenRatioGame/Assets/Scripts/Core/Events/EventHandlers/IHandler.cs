namespace TDS.Events
{
    public interface IHandler<in T>
    {
        void Handle(T operation);
    }
}