namespace IMBase
{
    public interface IFactory<out T>
    {
        T Create();
    }

    public interface IFactory<out T, in TParam1>
    {
        T Create(TParam1 param1);
    }

    public interface IFactory<out T, in TParam1, in TParam2>
    {
        T Create(TParam1 param1, TParam2 param2);
    }
}