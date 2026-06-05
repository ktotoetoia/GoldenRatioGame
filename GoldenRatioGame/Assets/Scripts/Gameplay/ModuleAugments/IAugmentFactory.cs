using IM.LifeCycle;

namespace IM.Augments
{
    public interface IAugmentFactory : IFactory<IAugment,IAugmentContext>
    {
        object Save(IAugment augment);
        IAugment Restore(object saved,IAugmentContext context);
    }
}