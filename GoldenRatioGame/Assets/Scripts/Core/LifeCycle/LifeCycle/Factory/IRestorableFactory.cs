namespace IM.LifeCycle
{
    public interface IRestorableFactory<TType, in TContext> : IFactory<TType, TContext>
    {
        object Save(TType augment);
        TType Restore(object saved,TContext context);
    }
}